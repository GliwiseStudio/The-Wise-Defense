using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamage, IDownStats
{
    #region Variables

    [SerializeField] private EnemyConfiguration _config;
    [SerializeField] private GameObject _summonedEnemy; // will only be used by summoner enemy

    private string _obstaclesLayerMask = "DamageableObstacles";

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

    private enum EnemyStates { walking, fighting, reaching }
    private EnemyStates _enemyState = EnemyStates.walking;

    private float _randomWaitTime;

    private EnemyHealth _enemyHealth;
    private EnemyMovement _enemyMovement;
    private TargetDetector _obstacleDetector;
    private AnimationsHandler _animationsHandler;
    private AudioPlayer _audioPlayer;

    #endregion

    private void TheStart() // called when instanciated from the wave spawner
    {
        GameManager.Instance.AddEnemy(); // add the enemy to the GameManager to keep track of it

        _randomWaitTime = Random.Range(0.0f, 1f);

        _speed = _config.Speed;
        _maxHealth = _config.MaxHealth;
        _detectionRange = _config.DetectionRange;
        _damage = _config.Damage;
        _damageAnimTime = _config.DamageAnimTime;
        _hitTime = _config.HitTime;
        _enemyType = _config.EnemyType;
        _summonerTime = _config.SummonerTime;
        _bomberDeathDamage = _config.BomberDeathDamage;

        _sceneCamera = FindObjectOfType<Camera>();
        _slider = GetComponentInChildren<Slider>();
        _materials = GetComponentInChildren<Renderer>().materials;
        _animator = GetComponent<Animator>();
        _audioPlayer = GetComponent<AudioPlayer>();

        _audioPlayer.ConfigureAudioSource(_config.AudioMixerChannel);

        _enemyHealth = new EnemyHealth(_maxHealth, _slider, _sceneCamera);
        _enemyMovement = new EnemyMovement(transform, _speed, _obstaclesLayerMask, _waypoints);
        _obstacleDetector = new TargetDetector(transform, _detectionRange, _obstaclesLayerMask);
        _animationsHandler = new AnimationsHandler(_animator);

        if(_enemyType == EnemyTypes.EnemyTypesEnum.summoner)
        {
            StartCoroutine(SummonEnemies());
        }
    }

    private void Update()
    {
        if (!_paralized)
        {
            if (_enemyHealth.GetEnemyState() == "alive")
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

            if(_enemyType == EnemyTypes.EnemyTypesEnum.armored) // armored enemy no longer fighting
            {
                gameObject.layer = LayerMask.NameToLayer("GroundEnemyArmored"); // change layer back to be indetectable
            }
        }
        else // if the enemy is neither walking nor fighting, it was still walking towards the obstacle when it dissapeared
        {
            _enemyMovement.ResetObstacleDetectionState();

            _enemyState = EnemyStates.walking;

            if (_enemyType == EnemyTypes.EnemyTypesEnum.armored) // armored enemy no longer fighting
            {
                gameObject.layer = LayerMask.NameToLayer("GroundEnemyArmored"); // change layer back to be indetectable
            }
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
        if(_targetTransform != null && _targetGameObject.layer == LayerMask.NameToLayer(_obstaclesLayerMask))
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
            int DeadEnemyLayer = LayerMask.NameToLayer("DeadEnemy");
            gameObject.layer = DeadEnemyLayer;

            _timeOfDeath = Time.time;
            _firstDeathCall = false;

            Destroy(_slider.gameObject);

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
            foreach(Material _mat in _materials)
            {
                _mat.SetFloat("_DissolveProgress", i);
            }
        }
        else // when a second has passed, destroy the gameObject
        {
            GameManager.Instance.RemoveEnemy();
            Destroy(gameObject);
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

        float animSpeed = _animator.GetFloat("Speed");
        animSpeed = animSpeed * (1 - slowdownPercentage);
        _animator.SetFloat("Speed", animSpeed);

        _damageAnimTime = _damageAnimTime / ( 1 - slowdownPercentage);
        _hitTime = _hitTime / (1 - slowdownPercentage);
    }

    public void ReleaseSlowdown(float slowdownPercentage)
    {
        _speed = _speed / (1 - slowdownPercentage);
        _enemyMovement.UpdateSpeed(_speed);

        float animSpeed = _animator.GetFloat("Speed");
        animSpeed = animSpeed / (1 - slowdownPercentage);
        _animator.SetFloat("Speed", animSpeed);

        _damageAnimTime = _damageAnimTime * (1 - slowdownPercentage);
        _hitTime = _hitTime * (1 - slowdownPercentage);
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
        ReceiveSlowdown(slowdownPercentage);
        ReceiveDamageReduction(damageReductionPercentage);
        StartCoroutine(ReleaseTimedDownStats(slowdownPercentage, damageReductionPercentage, duration));
    }

    IEnumerator ReleaseTimedDownStats(float slowdownPercentage, float damageReductionPercentage, float duration)
    {
        yield return new WaitForSeconds(duration);
        ReleaseSlowdown(slowdownPercentage);
        ReleaseDamageReduction(damageReductionPercentage);
    }

    public void ReceiveTimedParalysis(float duration)
    {
        // paralize enemy, change it's color to yellow and play idle animation
        _paralized = true;
        ChangeEnemyColor(new Color32(0, 201, 254, 1)); // color aqua
        PlayIdleAnimation();

        // start coroutine to release the paralysis
        StartCoroutine(ReleaseParalysis(duration));
    }

    IEnumerator ReleaseParalysis(float duration)
    {
        yield return new WaitForSeconds(duration);

        // after a few seconds, stop paralysis, change color back to normal, and continue normal animations
        _paralized = false;

        ChangeEnemyColor(new Color32(255, 255, 255, 1)); // color white

        if (_enemyState == EnemyStates.walking)
        {
            PlayWalkingAnimation();
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

    #region Setters
    public void SetWaypointsAndSpawnPoint(Waypoints waypoints, Transform spawnPoint)
    {
        _waypoints = waypoints;
        _spawnPoint = spawnPoint;
    }

    #endregion

    #region Enemy type particular methods
    IEnumerator SummonEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(_summonerTime);

            GameObject enemy = Instantiate(_summonedEnemy, _spawnPoint.position, _spawnPoint.rotation);
            enemy.GetComponent<EnemyController>().SetWaypointsAndSpawnPoint(_waypoints, _spawnPoint);
            enemy.SendMessage("TheStart", _waypoints);
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

}
