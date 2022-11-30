using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/Area Heal Power Configuration", fileName = "AreaHealPowerConfiguration")]
public class AreaHealPowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] [Range(0.1f, 1f)] private float _healPercentage = 0.5f;
    [SerializeField] private float _range = 5f;
    [SerializeField] private Color _color;
    [SerializeField] private string[] _targetLayerMasks;

    protected override ICardPower InitializePower()
    {
        return new AreaHealPower(_prefab, _healPercentage, _range, _color, _targetLayerMasks);
    }
}
