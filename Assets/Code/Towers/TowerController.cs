using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioPlayer))]
public class TowerController : MonoBehaviour
{
    [SerializeField] private TowerConfigurationSO _configuration;
    [SerializeField] private Transform _firingPointTransform;

    [SerializeField] private Animator _animator;

    private TowerHeadRotator _headRotator;
    private TargetDetector _enemyDetector;
    private TowerShootComponent _shootComponent;
    private Transform _targetTransform;
    private TowerAnimationsHandler _animationsHandler;
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _headRotator = new TowerHeadRotator(transform);
        _enemyDetector = new TargetDetector(transform, _configuration.DetectionRange, _configuration.TargetLayerMask);
        _shootComponent = new TowerShootComponent(FindObjectOfType<ProjectileSpawner>(), _configuration.FireRate, _configuration.ProjectileConfigurationSO);
        _animationsHandler = new TowerAnimationsHandler(_animator);
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void OnEnable()
    {
        _shootComponent.OnShotPerformed += PlayShootingAnimation;
        _shootComponent.OnShotPerformed += PlayShootSound;
    }

    private void OnDisable()
    {
        _shootComponent.OnShotPerformed -= PlayShootingAnimation;
        _shootComponent.OnShotPerformed -= PlayShootSound;
    }

    private void PlayShootingAnimation()
    {
        _animationsHandler.PlayAnimationState("Shoot", 0.1f);
    }

    private void PlayShootSound()
    {
        _audioPlayer.PlayAudio("CannonShot");
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
