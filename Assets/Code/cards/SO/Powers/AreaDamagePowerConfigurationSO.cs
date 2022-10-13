using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/Area Damage Power Configuration", fileName = "AreaDamagePowerConfiguration")]
public class AreaDamagePowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _damage = 30;
    [SerializeField] private float _range = 5f;
    [SerializeField] private string[] _targetLayerMasks;

    protected override ICardPower InitializePower()
    {
        return new AreaDamagePower(_prefab, _damage, _range, _targetLayerMasks);
    }
}
