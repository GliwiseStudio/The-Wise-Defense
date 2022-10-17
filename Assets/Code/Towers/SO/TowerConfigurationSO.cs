using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tower/TowerConfiguration", fileName = "TowerNameConfiguration")]
public class TowerConfigurationSO : ScriptableObject
{
    [SerializeField] private TowerDetectionConfiguration _detectionConfiguration;
    [SerializeField] private TowerShootingConfiguration _shootingConfiguration;
    [SerializeField] private TowerUpgradeList _upgradeList;

    [SerializeField] private TowerAudioConfiguration _audioConfiguration;

    [SerializeField] private ProjectileConfigurationSO _projectileConfiguration;

    public TowerDetectionConfiguration DetectionConfiguration => _detectionConfiguration;
    public TowerShootingConfiguration ShootingConfiguration => _shootingConfiguration;
    public TowerUpgradeList UpgradeList => _upgradeList;
    public TowerAudioConfiguration AudioConfiguration => _audioConfiguration;
    public ProjectileConfigurationSO ProjectileConfigurationSO => _projectileConfiguration;
}
