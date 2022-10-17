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

    public string GetName()
    {
        return _configuration.Name;
    }

    private void Awake()
    {
        _buffController = new TowerBuffController();
        _headRotator = new TowerHeadRotator(transform);
        _enemyDetector = new TargetDetector(transform, _configuration.DetectionConfiguration.DetectionRange, _configuration.DetectionConfiguration.TargetLayerMask);
        _shootComponent = new TowerShootComponent(FindObjectOfType<ProjectileSpawner>(), _configuration.ShootingConfiguration.Damage, _configuration.ShootingConfiguration.FireRate, _configuration.ProjectileConfigurationSO, _configuration.DetectionConfiguration.TargetLayerMask);
        _animationsHandler = new AnimationsHandler(_animator);
        _upgradeComponent = new TowerLevelUp(_configuration.UpgradeList);
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void OnEnable()
    {
        _shootComponent.OnShotPerformed += PlayShootingAnimation;
        _shootComponent.OnShotPerformed += PlayShootSound;

        _upgradeComponent.OnLevelUp += ApplyLevelUp;

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

        _upgradeComponent.OnLevelUp -= ApplyLevelUp;

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

    public void LevelUp()
    {
        _upgradeComponent.LevelUp();
    }

    private void ApplyLevelUp(TowerUpgrade upgrade)
    {
        _shootComponent.SetDamage(upgrade.Damage);
        _enemyDetector.SetRadius(upgrade.Range);
        _shootComponent.SetFirerate(upgrade.FireRate);
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

        _enemyDetector.SetRadius(_configuration.DetectionConfiguration.DetectionRange + ((_configuration.DetectionConfiguration.DetectionRange * buffPercentage) / 100));
    }

    private void UnbuffDetectionRange()
    {
        _enemyDetector.SetRadius(_configuration.DetectionConfiguration.DetectionRange);
    }
}
