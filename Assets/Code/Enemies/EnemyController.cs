using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamage
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _maxHealth = 500;
    [SerializeField] private float _detectionRange = 2f;
    [SerializeField] private int _damagePerSecond = 50;
    [SerializeField] private string _obstaclesLayerMask = "Obstacles";

    private Slider _slider;
    private Camera _sceneCamera;
    private Transform _targetTransform;
    private GameObject _targetGameObject;

    private bool _canDamage = true;
    private float _lastDamagedTime;

    private Material _mat;
    private bool _firstDeathCall = true;
    private float _timeOfDeath;

    private EnemyHealth _enemyHealth;
    private EnemyMovement _enemyMovement;
    private TargetDetector _obstacleDetector;

    private void Start()
    {
        _sceneCamera = FindObjectOfType<Camera>();
        _slider = GetComponentInChildren<Slider>();

        _enemyHealth = new EnemyHealth(_maxHealth, _slider, _sceneCamera);

        _enemyMovement = new EnemyMovement(transform, _speed);

        _obstacleDetector = new TargetDetector(transform, _detectionRange, _obstaclesLayerMask);

        _mat = GetComponent<Renderer>().material;
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

    public void ReceiveDamage(int damageAmount)
    {
        _enemyHealth.ReceiveDamage(damageAmount);
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
        if(Time.time - _timeOfDeath < 1) // makes Progress property of shader go from 1 to 0 in the span of 1 second
        {
            float i = 1 - (Time.time - _timeOfDeath);
            _mat.SetFloat("_Progress", i);
        }
        else // when a second has passed, destroy the gameObject
        {
            Destroy(gameObject);
        }
        
    }
}
