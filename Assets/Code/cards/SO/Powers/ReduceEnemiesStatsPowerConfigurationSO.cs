using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/Reduce Enemies Stats Power Configuration", fileName = "ReduceEnemiesStatsPowerConfiguration")]
public class ReduceEnemiesStatsPowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _speed = 30;
    [SerializeField] private int _damage = 30;
    [SerializeField] private float _range = 5f;
    [SerializeField] private string[] _targetLayerMasks;

    protected override ICardPower InitializePower()
    {
        return new ReduceEnemiesStatsPower(_prefab, _speed, _damage, _range, _targetLayerMasks);
    }
}
