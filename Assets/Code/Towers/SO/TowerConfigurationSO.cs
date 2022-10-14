using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tower/TowerConfiguration", fileName = "TowerNameConfiguration")]
public class TowerConfigurationSO : ScriptableObject
{
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private string[] _targetLayerMask;
    [SerializeField] private TowerShootingConfiguration _shootingConfiguration;
    [SerializeField] private TowerUpgradeList _upgradeList;

    [SerializeField] private TowerAudioConfiguration _audioConfiguration;

    [SerializeField] private ProjectileConfigurationSO _projectileConfiguration;

    public float DetectionRange => _detectionRange;
    public string[] TargetLayerMask => _targetLayerMask;
    public TowerShootingConfiguration ShootingConfiguration;
    public TowerUpgradeList UpgradeList => _upgradeList;
    public TowerAudioConfiguration AudioConfiguration => _audioConfiguration;
    public ProjectileConfigurationSO ProjectileConfigurationSO => _projectileConfiguration;
}
