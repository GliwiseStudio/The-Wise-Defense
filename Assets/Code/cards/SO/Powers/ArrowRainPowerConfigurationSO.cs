using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/Arrow Rain Power Configuration", fileName = "ArrowRainPowerConfiguration")]
public class ArrowRainPowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _damage = 30;
    [SerializeField] private float _range = 5f;
    [SerializeField] private string[] _targetLayerMasks;

    protected override ICardPower InitializePower()
    {
        return new ArrowRainPower(_prefab, _damage, _range, _targetLayerMasks);
    }
}
