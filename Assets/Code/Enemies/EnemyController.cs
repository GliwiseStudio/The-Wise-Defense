using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamage, ISlowdown
{
    #region Variables

    [SerializeField] private EnemyConfiguration _config;
    
    private string _obstaclesLayerMask = "DamageableObstacles";

    private float _speed;
    private float _maxHealth;
    private float _detectionRange;
    private int _damage;
    private float _damageAnimTime;
    private float _hitTime;

    private Slider _slider;
    private Camera _sceneCamera;
    private Transform _targetTransform;
    private GameObject _targetGameObject;
    private Animator _animator;

    private bool _canDamage = true;
    private float _lastDamagedTime;

    private Material[] _materials;
    private bool _firstDeathCall = true;
    private float _timeOfDeath;
    private bool _fightingStateChange = false;
    private bool _walkingStateChange = true;
    private float _randomWaitTime;

    private EnemyHealth _enemyHealth;
    private EnemyMovement _enemyMovement;
    private TargetDetector _obstacleDetector;
    private AnimationsHandler _animationsHandler;
    private AudioPlayer _audioPlayer;

    #endregion

    private void Start()
    {
        _randomWaitTime = Random.Range(0.0f, 1.5f);

        _speed = _config.Speed;
        _maxHealth = _config.MaxHealth;
        _detectionRange = _config.DetectionRange;
        _damage = _config.Damage;
        _damageAnimTime = _config.DamageAnimTime;
        _hitTime = _config.HitTime;

        _sceneCamera = FindObjectOfType<Camera>();
        _slider = GetComponentInChildren<Slider>();
        _materials = GetComponentInChildren<Renderer>().materials;
        _animator = GetComponent<Animator>();
        _audioPlayer = GetComponent<AudioPlayer>();

        _enemyHealth = new EnemyHealth(_maxHealth, _slider, _sceneCamera);
        _enemyMovement = new EnemyMovement(transform, _speed);
        _obstacleDetector = new TargetDetector(transform, _detectionRange, _obstaclesLayerMask);
        _animationsHandler = new AnimationsHandler(_animator);
    }

    private void Update()
    {
        if (_enemyHealth.GetEnemyState() == "alive")
        {
            _enemyHealth.Update();
            _enemyMovement.Update();

            if (_targetTransform != null) // if there's an obstacle
            {
                ObstacleDetected();
            }
            else // no obstacles
            {
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

    #region Movement
    private void KeepWalking()
    {
        if (_fightingStateChange)
        {
            PlayIdleAnimation();

            if (Time.time - _lastDamagedTime >= _randomWaitTime)
            {
                _enemyMovement.CalculateWaypointDirection();

                PlayWalkingAnimation();

                _fightingStateChange = false;
                _walkingStateChange = true;
            }
        }
    }
    #endregion

    #region DamageableObstacles related methods

    private void ObstacleDetected()
    {
        // obstacles don't move, so once it sees a target it's always in range

        if (_walkingStateChange)
        {
            _enemyMovement.CalculateObstacleDirection(_targetTransform);

            if (_enemyMovement.ObstacleReached)
            {
                PlayFightingAnimation();

                _fightingStateChange = true;
                _walkingStateChange = false;
            }
        }

        DamageControlUpdate();

        if (_canDamage) // check if enough time has passed to damage the obstacle again
        {
            //PlayFightingAnimation();
            _canDamage = false;
            _lastDamagedTime = Time.time;
            StartCoroutine(PlayDamage());
        }
    }

    IEnumerator PlayDamage()
    {
        yield return new WaitForSeconds(_hitTime);
        if(_targetTransform != null)
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

}
