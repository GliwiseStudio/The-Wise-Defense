using UnityEngine;

[DisallowMultipleComponent]
public class TowerController : MonoBehaviour
{
    [SerializeField] private TowerConfigurationSO _configuration;
    [SerializeField] private Transform _firingPointTransform;

    private TowerHeadRotator _headRotator;
    private TargetDetector _enemyDetector;
    private TowerShootComponent _shootComponent;
    private Transform _targetTransform;

    private void Awake()
    {
        _headRotator = new TowerHeadRotator(transform);
        _enemyDetector = new TargetDetector(transform, _configuration.DetectionRange, _configuration.TargetLayerMask);
        _shootComponent = new TowerShootComponent(FindObjectOfType<ProjectileSpawner>(), _configuration.FireRate, _configuration.ProjectileConfigurationSO);
    }

    private void Update()
    {
        _shootComponent.Update();

        if (_targetTransform != null)
        {
            if (_enemyDetector.IsTargetInRange(_targetTransform.position))
            {
                _headRotator.Update(_targetTransform);
                _shootComponent.Shoot(_firingPointTransform.position, transform.forward, _targetTransform);
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
