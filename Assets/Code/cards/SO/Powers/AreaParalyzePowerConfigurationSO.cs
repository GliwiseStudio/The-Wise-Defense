using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/Area Paralyze Power Configuration", fileName = "AreaParalyzePowerConfiguration")]
public class AreaParalyzePowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _speed = 30;
    [SerializeField] private float _range = 5f;
    [SerializeField] private string[] _targetLayerMasks;

    protected override ICardPower InitializePower()
    {
        return new AreaParalyzePower(_prefab, _speed, _range, _targetLayerMasks);
    }
}
