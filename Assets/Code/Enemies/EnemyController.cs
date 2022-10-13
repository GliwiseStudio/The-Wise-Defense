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
    private float _damageTime;

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

    private EnemyHealth _enemyHealth;
    private EnemyMovement _enemyMovement;
    private TargetDetector _obstacleDetector;
    private AnimationsHandler _animationsHandler;

    #endregion

    private void Start()
    {
        _speed = _config.Speed;
        _maxHealth = _config.MaxHealth;
        _detectionRange = _config.DetectionRange;
        _damage = _config.Damage;
        _damageTime = _config.DamageTime;

        _sceneCamera = FindObjectOfType<Camera>();
        _slider = GetComponentInChildren<Slider>();
        _materials = GetComponentInChildren<Renderer>().materials;
        _animator = GetComponent<Animator>();

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

            if (_targetTransform != null)
            {
                DamageControlUpdate();
                // obstacles don't move, so once it sees a target it's always in range

                if (_walkingStateChange)
                {
                    PlayFightingAnimation();
                    _fightingStateChange = true;
                    _walkingStateChange = false;
                }

                if (_canDamage) // check if enough time has passed to damage the obstacle again
                {
                    _canDamage = false;
                    _lastDamagedTime = Time.time;

                    IDamage obstacleInterface = _targetGameObject.GetComponent<IDamage>();
                    obstacleInterface.ReceiveDamage(_damage);
                }
            }

            else
            {
                if (_fightingStateChange)
                {
                    PlayWalkingAnimation();
                    _fightingStateChange = false;
                    _walkingStateChange = true;
                }

                _targetGameObject = _obstacleDetector.DetectTargetGameObject();
                _targetTransform = _obstacleDetector.DetectTarget();
                
                _enemyMovement.Update();
            }
        }
        else
        {
            if(_firstDeathCall == true)
            {
                _timeOfDeath = Time.time;
                _firstDeathCall = false;
            }

            Dissolve();
        }
    }

    private void DamageControlUpdate()
    {
        if (!_canDamage)
        {
            if (Time.time - _lastDamagedTime >= _damageTime) // if a second has passed
            {
                _canDamage = true;
            }
        }
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

    #region Interface methods
    public void ReceiveDamage(int damageAmount)
    {
        _enemyHealth.ReceiveDamage(damageAmount);
    }

    public void ReceiveSlowdown(float slowdown)
    {
        _speed -= slowdown;
        _enemyMovement.UpdateSpeed(_speed);
    }

    public void ReleaseSlowdown(float slowdown)
    {
        _speed += slowdown;
        _enemyMovement.UpdateSpeed(_speed);
    }

    #endregion

    #region AnimationsHandler functions
    private void PlayFightingAnimation()
    {
        _animationsHandler.PlayAnimationState("Fighting", 0.1f);
    }

    private void PlayWalkingAnimation()
    {
        _animationsHandler.PlayAnimationState("Walking", 0.1f);
    }

    #endregion

}
