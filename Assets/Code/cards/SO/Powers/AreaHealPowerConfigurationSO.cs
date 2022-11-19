using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/Area Heal Power Configuration", fileName = "AreaHealPowerConfiguration")]
public class AreaHealPowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _health = 30;
    [SerializeField] private float _range = 5f;
    [SerializeField] private Color _color;
    [SerializeField] private string[] _targetLayerMasks;

    protected override ICardPower InitializePower()
    {
        return new AreaHealPower(_prefab, _health, _range, _color, _targetLayerMasks);
    }
}
