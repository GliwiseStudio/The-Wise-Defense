using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/Area Paralyze Power Configuration", fileName = "AreaParalyzePowerConfiguration")]
public class AreaParalyzePowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private float _range = 5f;
    [SerializeField] private Color _color;
    [SerializeField] private string[] _targetLayerMasks;

    protected override ICardPower InitializePower()
    {
        return new AreaParalyzePower(_prefab, _duration, _range, _color, _targetLayerMasks);
    }
}
