using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/Reduce Enemies Stats Power Configuration", fileName = "ReduceEnemiesStatsPowerConfiguration")]
public class ReduceEnemiesStatsPowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] [Range(0.1f, 0.9f)] private float _speedReductionPercentage = 0.5f;
    [SerializeField] [Range(0.1f, 0.9f)] private float _damageReductionPercentage = 0.5f;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private float _range = 5f;
    [SerializeField] private Color _color;
    [SerializeField] private string[] _targetLayerMasks;

    protected override ICardPower InitializePower()
    {
        return new ReduceEnemiesStatsPower(_prefab, _speedReductionPercentage, _damageReductionPercentage, _duration, _range, _color, _targetLayerMasks);
    }
}
