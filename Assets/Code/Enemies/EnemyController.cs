using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, EnemyInterface
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _maxHealth = 500;
    [SerializeField] private float _detectionRange = 2f;
    [SerializeField] private string _obstaclesLayerMask = "Obstacles";

    private Slider _slider;
    private Camera _sceneCamera;
    private Transform _targetTransform;

    private EnemyHealth _enemyHealth;
    private EnemyMovement _enemyMovement;
    private TargetDetector _obstacleDetector;

    private void Start()
    {
        _sceneCamera = FindObjectOfType<Camera>();
        _slider = _slider = GetComponentInChildren<Slider>();

        _enemyHealth = new EnemyHealth(_maxHealth, _slider, _sceneCamera);

        _enemyMovement = new EnemyMovement(transform, _speed);

        _obstacleDetector = new TargetDetector(transform, _detectionRange, _obstaclesLayerMask);
    }

    private void Update()
    {
        if(_enemyHealth.GetEnemyState() == "alive")
        {
            _enemyHealth.Update();

            if (_targetTransform != null)
            {
                if (_obstacleDetector.IsTargetInRange(_targetTransform.position))
                {
                    Debug.Log("Shooting");
                }
                else
                {
                    _targetTransform = null;
                    _enemyMovement.Update();

                }
            }
            else
            {
                _targetTransform = _obstacleDetector.DetectTarget(); 
                _enemyMovement.Update();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReceiveDamage(int damageAmount)
    {
        _enemyHealth.ReceiveDamage(damageAmount);
    }
}
