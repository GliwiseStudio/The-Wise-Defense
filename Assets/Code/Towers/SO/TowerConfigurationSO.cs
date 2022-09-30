using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tower/TowerConfiguration", fileName = "TowerNameConfiguration")]
public class TowerConfigurationSO : ScriptableObject
{
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private string _targetLayerMask = "Enemies";
    [SerializeField] private float _fireRate = 0.8f;

    [SerializeField] private ProjectileConfigurationSO _projectileConfiguration;

    public float DetectionRange => _detectionRange;
    public string TargetLayerMask => _targetLayerMask;
    public float FireRate => _fireRate;
    public ProjectileConfigurationSO ProjectileConfigurationSO => _projectileConfiguration;
}
