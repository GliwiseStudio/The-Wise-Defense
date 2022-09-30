using UnityEngine;

[DisallowMultipleComponent]
public class TowerController : MonoBehaviour
{
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private string _enemiesLayerMask = "Enemies";
    [SerializeField] private string _projectileType = "Arrow";
    [SerializeField] private float _fireRate = 0.8f;
    [SerializeField] private Transform _firingPointTransform;

    private TowerHeadRotator _headRotator;
    private TargetDetector _enemyDetector;
    private TowerShootComponent _shootComponent;
    private Transform _targetTransform;

    private void Awake()
    {
        _headRotator = new TowerHeadRotator(transform);
        _enemyDetector = new TargetDetector(transform, _detectionRange, _enemiesLayerMask);
        _shootComponent = new TowerShootComponent(FindObjectOfType<ProjectileSpawner>(), _projectileType, _fireRate);
    }

    private void Update()
    {
        _shootComponent.Update();

        if (_targetTransform != null)
        {
            if (_enemyDetector.IsTargetInRange(_targetTransform.position))
            {
                _headRotator.Update(_targetTransform);
                _shootComponent.Shoot(_firingPointTransform.position, transform.forward);
            }
            else
            {
                _targetTransform = null;
            }
        }
        else
        {
            _targetTransform = _enemyDetector.DetectTarget();
        }
    }
}
