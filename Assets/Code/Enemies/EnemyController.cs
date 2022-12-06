using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : RecyclableObject, IDamage, IDownStats
{
    #region Variables

    public EnemyConfiguration _config;
    [SerializeField] private GameObject _summonedEnemy; // will only be used by summoner enemy

    private string _obstaclesLayerMask = "DamageableObstacles";

    private float _animSpeed;
    private float _speed;
    private float _maxHealth;
    private float _detectionRange;
    private int _damage;
    private float _damageAnimTime;
    private float _hitTime;
    private EnemyTypes.EnemyTypesEnum _enemyType;
    private int _summonerTime; // will only be used by summoner enemy
    private int _bomberDeathDamage; // will only be used by bomber enemy

    private Waypoints _waypoints;
    private Transform _spawnPoint; // will be used by summoner enemy to spawn more enemies
    private Slider _slider;
    private Camera _sceneCamera;
    private Transform _targetTransform;
    private GameObject _targetGameObject;
    private Animator _animator;
    private Material[] _materials;

    private bool _canDamage = true;
    private float _lastDamagedTime;
    private bool _firstDeathCall = true;
    private float _timeOfDeath;
    private bool _onDamageLoop = false;
    private bool _paralized = false;
    private bool _timedDown = false;

    private GameObject _currentSlowdownObstacle = null;

    private enum EnemyStates { walking, fighting, reaching }
    private EnemyStates _enemyState = EnemyStates.walking;

    private float _randomWaitTime;

    private EnemyHealth _enemyHealth;
    private EnemyMovement _enemyMovement;
    private TargetDetector _obstacleDetector;
    private AnimationsHandler _animationsHandler;
    private AudioPlayer _audioPlayer;

    private bool _started = false;

    private EnemySpawner _enemySpawner;

    #endregion

    private void Awake() // called when instanciated for the first time
    {
        // this are only assigned once, at the start
        _randomWaitTime = Random.Range(0.0f, 1f);


        _slider = GetComponentInChildren<Slider>();
        _materials = GetComponentInChildren<Renderer>().materials;
        _animator = GetComponent<Animator>();
        _animSpeed = 1;
        _animator.SetFloat("Speed", _animSpeed);

        // config variables (always the same because they are dependant of the enemy type)
        _speed = _config.Speed;
        _maxHealth = _config.MaxHealth;
        _detectionRange = _config.DetectionRange;
        _damage = _config.Damage;
        _damageAnimTime = _config.DamageAnimTime;
        _hitTime = _config.HitTime;
        _enemyType = _config.EnemyType;
        _summonerTime = _config.SummonerTime;
        _bomberDeathDamage = _config.BomberDeathDamage;
        gameObject.layer = LayerMask.NameToLayer(_config.LayerName);

        // created just once, then we'll need to reset them
        _enemyHealth = new EnemyHealth(_maxHealth, _slider);
        _enemyMovement = new EnemyMovement(transform, _obstaclesLayerMask);
        _obstacleDetector = new TargetDetector(transform, _detectionRange, _obstaclesLayerMask); // not necessary to reset, it's passed parameters don't change
        _animationsHandler = new AnimationsHandler(_animator); // not necessary to reset, it's passed parameters don't change
        Debug.Log(_enemyHealth);
    }

    private void Start()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
        _audioPlayer.ConfigureAudioSource(_config.AudioMixerChannel);
    }

    #region Listeners

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEndScene += ResetEnemy;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEndScene -= ResetEnemy;
        }
    }

    #endregion

    #region Recyclable object related methods

    public override void Init() { }
    private void ResetEnemy()
    {
        Recycle();
    }

    public override void Release()
    {
        // reset speed
        _speed = _config.Speed;
        _animSpeed = 1;
        _animator.SetFloat("Speed", _animSpeed);

        // reset layer
        gameObject.layer = LayerMask.NameToLayer(_config.LayerName);

        // reset seen obstacle
        _targetGameObject = null;
        _targetTransform = null;

        // reset booleans to init state
        _started = false;
        _canDamage = true;
        _firstDeathCall = true;
        _onDamageLoop = false;
        _paralized = false;
        _timedDown = false;

        // reset slider
        _slider.gameObject.SetActive(true);

        // reset enemyState
        _enemyState = EnemyStates.walking;

        // reset materials
        foreach (Material _mat in _materials)
        {
            _mat.SetFloat("_DissolveProgress", 0);
        }

        ChangeEnemyColor(new Color32(255, 255, 255, 1)); // color white
    }

    #endregion

    public void Initialize(Waypoints waypoints, Transform spawnPoint)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.AddEnemy(); // add the enemy to the GameManager to keep track of it

        _waypoints = waypoints;
        _spawnPoint = spawnPoint;

        // reset other scripts
        Debug.Log(_enemyHealth);
        _sceneCamera = FindObjectOfType<Camera>();
        _enemyHealth.Reset(_sceneCamera);
        _enemyMovement.Reset(_speed, _waypoints);

        if (_enemyType == EnemyTypes.EnemyTypesEnum.summoner)
        {
            _enemySpawner = FindObjectOfType<EnemySpawner>();
            StartCoroutine(SummonEnemies());
        }

        _started = true;
    }

    private void Update()
    {
        if (_started)
        {
            if (_enemyHealth.GetEnemyState() == "alive")
            {
                if (!_paralized) // if enemy is paralized do not execute movement
                {
                    _enemyHealth.Update();
                    _enemyMovement.Update();

                    if (_targetTransform != null && _targetGameObject.layer == LayerMask.NameToLayer(_obstaclesLayerMask)) // if there's an obstacle
                    {
                        ObstacleDetected();
                    }
                    else // no obstacles
                    {
                        if (_enemyState != EnemyStates.walking)
                            KeepWalking();

                        // detect if there's an obstacle in range
                        _targetGameObject = _obstacleDetector.DetectTargetGameObject();
                        _targetTransform = _obstacleDetector.DetectTarget();
                    }
                }
            }
            else
            {
                EnemyDeath();
            }
        }
    }

    #region Movement
    private void KeepWalking()
    {
        if (_enemyState == EnemyStates.fighting) // the player was fighting when the obstacle dissapeared
        {
            PlayIdleAnimation();

            if (Time.time - _lastDamagedTime >= _randomWaitTime) // wait a random time to keep walking
            {
                _enemyMovement.ResetObstacleDetectionState();

                PlayWalkingAnimation();

                _enemyState = EnemyStates.walking;
            }

            if (_enemyType == EnemyTypes.EnemyTypesEnum.armored) // armored enemy no longer fighting
            {
                gameObject.layer = LayerMask.NameToLayer("GroundEnemyArmored"); // change layer back to be indetectable
            }
        }
        else // if the enemy is neither walking nor fighting, it was still walking towards the obstacle when it dissapeared
        {
            _enemyMovement.ResetObstacleDetectionState();

            _enemyState = EnemyStates.walking;
        }
    }
    #endregion

    #region DamageableObstacles related methods

    private void ObstacleDetected()
    {
        // obstacles don't move, so once it sees a target it's always in range

        if (_enemyState == EnemyStates.walking) // first time it enters the method after detecting an obstacle
        {
            _enemyState = EnemyStates.reaching;
            _enemyMovement.ObstacleDetected = true;

        }

        if (_enemyState == EnemyStates.reaching) // the enemy is walking towards the obstacle, but has not reached it yet
        {
            if (_enemyMovement.ObstacleReached)
            {
                _enemyState = EnemyStates.fighting;

                if (_enemyType == EnemyTypes.EnemyTypesEnum.armored) // armored enemy is fighting
                {
                    gameObject.layer = LayerMask.NameToLayer("GroundEnemies"); // change layer to be detectable by weapons
                }
            }
        }

        if (_enemyState == EnemyStates.fighting) // obstacle reached
        {
            DamageControlUpdate();

            if (_canDamage) // check if enough time has passed to damage the obstacle again
            {
                PlayFightingAnimation();
                _canDamage = false;
                _lastDamagedTime = Time.time;
                StartCoroutine(PlayDamage());
            }
        }
    }

    IEnumerator PlayDamage()
    {
        yield return new WaitForSeconds(_hitTime);

        // check that: the enemy is still alive, and not dead but dissolving
        // the obstacle still exits, it hasn't already been destroyed
        // if the obstacle exits, it is still damageable, and not dissolving
        if (_enemyHealth.GetEnemyState() == "alive" && _targetTransform != null && _targetGameObject.layer == LayerMask.NameToLayer(_obstaclesLayerMask) && !_paralized)
        {
            _audioPlayer.PlayAudio("Punch");

            IDamage obstacleInterface = _targetGameObject.GetComponent<IDamage>();
            obstacleInterface.ReceiveDamage(_damage);
        }
    }

    private void DamageControlUpdate()
    {
        if (!_canDamage)
        {
            if (Time.time - _lastDamagedTime >= _damageAnimTime) // if a second has passed
            {
                _canDamage = true;
            }
        }
    }
    #endregion

    #region Death methods

    private void EnemyDeath()
    {
        if (_firstDeathCall == true)
        {
            _animator.SetFloat("Speed", 0);

            int DeadEnemyLayer = LayerMask.NameToLayer("DeadEnemy");
            gameObject.layer = DeadEnemyLayer;

            _timeOfDeath = Time.time;
            _firstDeathCall = false;

            _slider.gameObject.SetActive(false);

            if (_enemyType == EnemyTypes.EnemyTypesEnum.bomber)
            {
                BomberDeath();
            }
        }

        Dissolve();
    }
    private void Dissolve()
    {
        if (Time.time - _timeOfDeath < 1) // makes Progress property of shader go from 1 to 0 in the span of 1 second
        {
            float i = Time.time - _timeOfDeath;
            foreach (Material _mat in _materials)
            {
                _mat.SetFloat("_DissolveProgress", i);
            }
        }
        else // when a second has passed, destroy the gameObject
        {
            if (GameManager.Instance != null)
                GameManager.Instance.RemoveEnemy();

            if (_currentSlowdownObstacle != null) // if it was inside of a slowdown obstacle when it died, remove it from the list
            {
                _currentSlowdownObstacle.gameObject.GetComponent<IRemove>().RemoveFromList(gameObject);
            }

            ResetEnemy();
        }
    }
    #endregion

    #region Interface methods
    public void ReceiveDamage(int damageAmount)
    {
        _enemyHealth.ReceiveDamage(damageAmount);
    }

    public void ReceiveSlowdown(float slowdownPercentage)
    {
        _speed = _speed * (1 - slowdownPercentage);
        _enemyMovement.UpdateSpeed(_speed);

        _animSpeed = _animSpeed * (1 - slowdownPercentage);
        _animator.SetFloat("Speed", _animSpeed);

        _damageAnimTime = _damageAnimTime / (1 - slowdownPercentage);
        _hitTime = _hitTime / (1 - slowdownPercentage);
    }

    public void ReleaseSlowdown(float slowdownPercentage)
    {
        _speed = _speed / (1 - slowdownPercentage);
        _enemyMovement.UpdateSpeed(_speed);

        _animSpeed = _animSpeed / (1 - slowdownPercentage);
        _animator.SetFloat("Speed", _animSpeed);

        _damageAnimTime = _damageAnimTime * (1 - slowdownPercentage);
        _hitTime = _hitTime * (1 - slowdownPercentage);
    }

    public void SetSlowdownObject(GameObject currentSlowdownObstacle)
    {
        _currentSlowdownObstacle = currentSlowdownObstacle;
    }

    public void RemoveSlowdownObject()
    {
        _currentSlowdownObstacle = null;
    }

    public void ReceiveDamageReduction(float damageReductionPercentage)
    {
        _damage = (int)(_damage * (1 - damageReductionPercentage));
    }

    public void ReleaseDamageReduction(float damageReductionPercentage)
    {
        _damage = (int)(_damage / (1 - damageReductionPercentage));
    }

    public void ReceiveTimedDownStats(float slowdownPercentage, float damageReductionPercentage, float duration)
    {
        _timedDown = true;
        ReceiveSlowdown(slowdownPercentage);
        ReceiveDamageReduction(damageReductionPercentage);
        ChangeEnemyColor(new Color32(254, 9, 0, 1)); // color red
        StartCoroutine(ReleaseTimedDownStats(slowdownPercentage, damageReductionPercentage, duration));
    }

    IEnumerator ReleaseTimedDownStats(float slowdownPercentage, float damageReductionPercentage, float duration)
    {
        yield return new WaitForSeconds(duration);

        _timedDown = false;
        ReleaseSlowdown(slowdownPercentage);
        ReleaseDamageReduction(damageReductionPercentage);

        if (!_paralized)
        {
            ChangeEnemyColor(new Color32(255, 255, 255, 1)); // color white
        }
        else
        {
            ChangeEnemyColor(new Color32(254, 224, 0, 1)); // color yellow
        }
    }

    public void ReceiveTimedParalysis(float duration)
    {
        // paralize enemy, change it's color to yellow and set animation speed to 0
        _paralized = true;
        ChangeEnemyColor(new Color32(254, 224, 0, 1)); // color yellow
        _animator.SetFloat("Speed", 0);

        // start coroutine to release the paralysis
        StartCoroutine(ReleaseParalysis(duration));
    }

    IEnumerator ReleaseParalysis(float duration)
    {
        yield return new WaitForSeconds(duration);

        // after a few seconds, stop paralysis, change color back to normal, and continue normal animations
        _paralized = false;
        _animator.SetFloat("Speed", _animSpeed);

        if (!_timedDown)
        {
            ChangeEnemyColor(new Color32(255, 255, 255, 1)); // color white
        }
        else
        {
            ChangeEnemyColor(new Color32(254, 9, 0, 1)); // color red
        }
    }

    public void ReceiveDamageOnLoop(int damage, float time)
    {
        _onDamageLoop = true;
        StartCoroutine(DamageLoop(damage, time));
    }

    public void StopDamageLoop()
    {
        _onDamageLoop = false;
    }

    IEnumerator DamageLoop(int damage, float time)
    {
        while (_onDamageLoop)
        {
            ReceiveDamage(damage);
            _audioPlayer.PlayAudio("Poison");
            StartCoroutine(TimedChangeEnemyColor(new Color32(143, 0, 254, 1))); // color purple
            yield return new WaitForSeconds(time);
        }
    }

    #endregion

    #region Color Change
    public void ChangeEnemyColor(Color32 color)
    {
        foreach (Material mat in _materials)
        {
            mat.SetColor("_MultiplyColor", color);
        }
    }

    IEnumerator TimedChangeEnemyColor(Color32 color)
    {
        ChangeEnemyColor(color);

        yield return new WaitForSeconds(0.25f);

        ChangeEnemyColor(new Color32(255, 255, 255, 1)); // color white
    }
    #endregion

    #region AnimationsHandler methods
    private void PlayFightingAnimation()
    {
        _animationsHandler.PlayAnimationState("Fighting", 0.1f);
    }

    private void PlayWalkingAnimation()
    {
        _animationsHandler.PlayAnimationState("Walking", 0.1f);
    }

    private void PlayIdleAnimation()
    {
        _animationsHandler.PlayAnimationState("Idle", 0.1f);
    }

    #endregion

    #region Enemy type particular methods
    IEnumerator SummonEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(_summonerTime);

            _enemySpawner.Spawn(EnemyTypes.EnemyTypesEnum.orc, _spawnPoint, _waypoints);
        }
    }

    private void BomberDeath()
    {
        GetComponentInChildren<ParticleSystem>().Play(true);

        IReadOnlyList<Transform> obstacles = _obstacleDetector.GetAllTargetsInRange();
        foreach (Transform obstacle in obstacles)
        {
            obstacle.gameObject.GetComponent<IDamage>().ReceiveDamage(_bomberDeathDamage);
        }
    }
    #endregion

    #region Getters
    public EnemyTypes.EnemyTypesEnum GetEnemyType()
    {
        return _config.EnemyType;
    }
    #endregion
}
