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
    private int _damagePerSecond;

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

    private EnemyHealth _enemyHealth;
    private EnemyMovement _enemyMovement;
    private TargetDetector _obstacleDetector;

    #endregion

    private void Start()
    {
        _speed = _config.Speed;
        _maxHealth = _config.MaxHealth;
        _detectionRange = _config.DetectionRange;
        _damagePerSecond = _config.DamagePerSecond;

        _sceneCamera = FindObjectOfType<Camera>();
        _slider = GetComponentInChildren<Slider>();
        _materials = GetComponentInChildren<Renderer>().materials;
        _animator = GetComponent<Animator>();

        _enemyHealth = new EnemyHealth(_maxHealth, _slider, _sceneCamera);
        _enemyMovement = new EnemyMovement(transform, _speed);
        _obstacleDetector = new TargetDetector(transform, _detectionRange, _obstaclesLayerMask);

    }

    private void Update()
    {
        DamageControlUpdate();

        if (_enemyHealth.GetEnemyState() == "alive")
        {
            _enemyHealth.Update();

            if (_targetTransform != null)
            {
                if (_obstacleDetector.IsTargetInRange(_targetTransform.position))
                {
                    _animator.SetBool("isFighting", true);
                    if (_canDamage)
                    {
                        _canDamage = false;
                        _lastDamagedTime = Time.time;

                        IDamage obstacleInterface = _targetGameObject.GetComponent<IDamage>();
                        obstacleInterface.ReceiveDamage(_damagePerSecond);
                    }
                }
                else
                {
                    _animator.SetBool("isFighting", false);
                    _targetTransform = null;
                    _enemyMovement.Update();

                }
            }
            else
            {
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
            if (Time.time - _lastDamagedTime >= 1) // if a second has passed
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
}
