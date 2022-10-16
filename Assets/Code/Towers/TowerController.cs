using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioPlayer))]
public class TowerController : MonoBehaviour, IBuff
{
    [SerializeField] private TowerConfigurationSO _configuration;
    [SerializeField] private Transform _firingPointTransform;

    [SerializeField] private Animator _animator;

    private TowerHeadRotator _headRotator;
    private TargetDetector _enemyDetector;
    private TowerShootComponent _shootComponent;
    private Transform _targetTransform;
    private GameObject _targetGameObject;
    private AnimationsHandler _animationsHandler;
    private AudioPlayer _audioPlayer;
    private TowerLevelUp _upgradeComponent;
    private TowerBuffController _buffController;

    private void Awake()
    {
        _buffController = new TowerBuffController();
        _headRotator = new TowerHeadRotator(transform);
        _enemyDetector = new TargetDetector(transform, _configuration.DetectionRange, _configuration.TargetLayerMask);
        _shootComponent = new TowerShootComponent(FindObjectOfType<ProjectileSpawner>(), _configuration.ShootingConfiguration, _configuration.ProjectileConfigurationSO, _configuration.TargetLayerMask);
        _animationsHandler = new AnimationsHandler(_animator);
        _upgradeComponent = new TowerLevelUp(_configuration.UpgradeList);
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Start()
    {
        _upgradeComponent.Start();
    }

    private void OnEnable()
    {
        _shootComponent.OnShotPerformed += PlayShootingAnimation;
        _shootComponent.OnShotPerformed += PlayShootSound;

        _upgradeComponent.OnLevelUp += LevelUp;

        _buffController.OnBuffDamage += BuffDamage;
        _buffController.OnUnbuffDamage += UnbuffDamage;
        _buffController.OnBuffFireRate += BuffFireRate;
        _buffController.OnUnbuffFireRate += UnbuffFireRate;
        _buffController.OnBuffDetectionRange += BuffDetectionRange;
        _buffController.OnUnbuffDetectionRange += UnbuffDetectionRange;
    }

    private void OnDisable()
    {
        _shootComponent.OnShotPerformed -= PlayShootingAnimation;
        _shootComponent.OnShotPerformed -= PlayShootSound;

        _upgradeComponent.OnLevelUp -= LevelUp;

        _buffController.OnBuffDamage -= BuffDamage;
        _buffController.OnUnbuffDamage -= UnbuffDamage;
        _buffController.OnBuffFireRate -= BuffFireRate;
        _buffController.OnUnbuffFireRate -= UnbuffFireRate;
        _buffController.OnBuffDetectionRange += BuffDetectionRange;
        _buffController.OnUnbuffDetectionRange += UnbuffDetectionRange;
    }

    private void PlayShootingAnimation()
    {
        _animationsHandler.PlayAnimationState("Shoot", 0.1f);
    }

    private void PlayShootSound()
    {
        _audioPlayer.PlayAudio(_configuration.AudioConfiguration.ShotSound);
    }

    private void LevelUp(TowerUpgrade upgrade)
    {

    }

    private void Update()
    {
        _buffController.Update();
        _shootComponent.Update();

        if (_targetTransform != null && _targetGameObject.layer != LayerMask.NameToLayer("DeadEnemy"))
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
            _targetGameObject = _enemyDetector.DetectTargetGameObject();
            _targetTransform = _enemyDetector.DetectTarget();
        }
    }  

    public void Buff(BuffKeyValue[] buffs)
    {
        _buffController.AddBuffs(buffs);
    }

    private void BuffDamage(int damage)
    {
        _shootComponent.BuffDamage(damage);
    }

    private void UnbuffDamage()
    {
        _shootComponent.UnbuffDamage();
    }

    private void BuffFireRate(int buffPercentage)
    {
        _shootComponent.BuffFireRate(buffPercentage);
    }

    private void UnbuffFireRate()
    {
        _shootComponent.UnbuffFireRate();
    }

    private void BuffDetectionRange(int buffPercentage)
    {

        _enemyDetector.SetRadius(_configuration.DetectionRange + ((_configuration.DetectionRange * buffPercentage) / 100));
    }

    private void UnbuffDetectionRange()
    {
        _enemyDetector.SetRadius(_configuration.DetectionRange);
    }
}
