using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioPlayer))]
public class TowerController : MonoBehaviour, IBuff
{
    [SerializeField] private TowerConfigurationSO _configuration;
    [SerializeField] private Transform _firingPointTransform;

    [SerializeField] private Animator _animator;

    [SerializeField] private List<GameObject> _visualsUpgrades;

    [Header("Visual help for players")]
    [SerializeField] private GameObject _rangeCylinder;
    [SerializeField] private Image _buffIconTemplate;
    [SerializeField] private Transform _buffIconParent;
    [SerializeField] private Canvas _buffIconsCanvas;

    private TowerHeadRotator _headRotator;
    private TargetDetector _enemyDetector;
    private TowerShootComponent _shootComponent;
    private Transform _targetTransform;
    private Collider _targetCollider;
    private GameObject _targetGameObject;
    private AnimationsHandler _animationsHandler;
    private AudioPlayer _audioPlayer;
    private TowerLevelUp _upgradeComponent;
    private TowerBuffController _buffController;
    private TowerVisualSwitcher _visualsSwitcher;

    private float _trueCurrentDetectionRange;
    private List<Image> _buffIcons;
    private bool _damageBuffed = false;
    private bool _rangeBuffed = false;
    private bool _fireRateBuffed = false;

    private Camera _camera;

    private IconsStorage _iconStorage;

    private ParticleSystem _particlesLevelUp;

    public string GetName()
    {
        return _configuration.Name;
    }

    public bool IsInMaximumLevel()
    {
        return _upgradeComponent.IsInMaximumLevel();
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
        _visualsSwitcher = new TowerVisualSwitcher(_visualsUpgrades, _animationsHandler);
        _buffIcons = new List<Image>();
        _camera = FindObjectOfType<Camera>();
        _particlesLevelUp = GetComponentInChildren<ParticleSystem>();
        _iconStorage = FindObjectOfType<IconsStorage>();

        _trueCurrentDetectionRange = _configuration.DetectionConfiguration.DetectionRange; // current detection range

        if (_rangeCylinder != null) {
            _rangeCylinder.transform.SetParent(null);
            SetRangeCylinderScale();
            _rangeCylinder.SetActive(false);
        }
    }

    private void Start()
    {
        _audioPlayer.ConfigureAudioSource(_configuration.AudioConfiguration.AudioMixerChannel);
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
        _audioPlayer.PlayAudio(_configuration.AudioConfiguration.LevelUpSound);
        _visualsSwitcher.SwitchVisuals(upgrade.Level);
        _shootComponent.SetDamage(upgrade.Damage);
        _enemyDetector.SetRadius(upgrade.Range);
        _shootComponent.SetFirerate(upgrade.FireRate);

        _trueCurrentDetectionRange = upgrade.Range; // new range to use

        if (_rangeCylinder != null)
            _rangeCylinder.transform.localScale = new Vector3(2 * _trueCurrentDetectionRange, 1, 2 * _trueCurrentDetectionRange); // change rage cylinder back

        _particlesLevelUp.Play();

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
                _shootComponent.Shoot(_firingPointTransform.position, transform.forward, _targetCollider);
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
            _targetCollider = _enemyDetector.DetectTargetCollider();
        }

        _buffIconsCanvas.gameObject.transform.LookAt(_camera.transform);
    }  

    public void Buff(BuffKeyValue[] buffs)
    {
        _buffController.AddBuffs(buffs);
    }

    private void BuffDamage(int damage)
    {
        _shootComponent.BuffDamage(damage);

        if (!_damageBuffed)
        {
            Image buffIcon = Instantiate(_buffIconTemplate);
            buffIcon.sprite = _iconStorage.GetSpriteFromCardStatType(CardStatType.DamageBuff);
            buffIcon.transform.SetParent(_buffIconParent, false);
            _buffIcons.Add(buffIcon);

            _damageBuffed = true;
        }
    }

    private void UnbuffDamage()
    {
        _shootComponent.UnbuffDamage();

        for(int i = 0; i < _buffIcons.Count; i++)
        {
            if (_buffIcons[i].sprite == _iconStorage.GetSpriteFromCardStatType(CardStatType.DamageBuff))
            {
                Destroy(_buffIcons[i].gameObject);
                _buffIcons.RemoveAt(i);

                _damageBuffed = false;
                break;
            }
        }
    }

    private void BuffFireRate(int buffPercentage)
    {
        _shootComponent.BuffFireRate(buffPercentage);


        if (!_fireRateBuffed)
        {
            Image buffIcon = Instantiate(_buffIconTemplate);
            buffIcon.sprite = _iconStorage.GetSpriteFromCardStatType(CardStatType.FireRateBuff);
            buffIcon.transform.SetParent(_buffIconParent, false);
            _buffIcons.Add(buffIcon);

            _fireRateBuffed = true;
        }
    }

    private void UnbuffFireRate()
    {
        _shootComponent.UnbuffFireRate();

        for (int i = 0; i < _buffIcons.Count; i++)
        {
            if (_buffIcons[i].sprite == _iconStorage.GetSpriteFromCardStatType(CardStatType.FireRateBuff))
            {
                Destroy(_buffIcons[i].gameObject);
                _buffIcons.RemoveAt(i);
                
                _fireRateBuffed = false;
                break;
            }
        }
    }

    private void BuffDetectionRange(int buffPercentage)
    {
        float newRadius = _trueCurrentDetectionRange + ((_trueCurrentDetectionRange * buffPercentage) / 100);

        _enemyDetector.SetRadius(newRadius);

        if (_rangeCylinder != null)
            _rangeCylinder.transform.localScale = new Vector3(2 * newRadius, 1, 2 * newRadius); // change rage cylinder to match buffered range

        if (!_rangeBuffed)
        {
            Image buffIcon = Instantiate(_buffIconTemplate);
            buffIcon.sprite = _iconStorage.GetSpriteFromCardStatType(CardStatType.RangeBuff);
            buffIcon.transform.SetParent(_buffIconParent, false);
            _buffIcons.Add(buffIcon);

            _rangeBuffed = true;
        }
    }

    private void UnbuffDetectionRange()
    {
        _enemyDetector.SetRadius(_trueCurrentDetectionRange);

        if (_rangeCylinder != null)
            SetRangeCylinderScale();

        for (int i = 0; i < _buffIcons.Count; i++)
        {
            if (_buffIcons[i].sprite == _iconStorage.GetSpriteFromCardStatType(CardStatType.RangeBuff))
            {
                Destroy(_buffIcons[i].gameObject);
                _buffIcons.RemoveAt(i);

                _rangeBuffed = false;
                break;
            }
        }
    }

    private void OnMouseDown()
    {
        if (_rangeCylinder != null)
            _rangeCylinder.SetActive(true);
    }

    private void OnMouseUp()
    {
        if (_rangeCylinder != null)
            _rangeCylinder.SetActive(false);
    }

    private void SetRangeCylinderScale()
    {
        _rangeCylinder.transform.localScale = new Vector3(2 * _trueCurrentDetectionRange, 1, 2 * _trueCurrentDetectionRange); // range cylinder scales is double the detection range
    }
}
