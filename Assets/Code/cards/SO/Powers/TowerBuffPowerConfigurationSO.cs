using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/Tower Buff PowerConfiguration", fileName = "TowerBuffPowerConfiguration")]
public class TowerBuffPowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private BuffKeyValue[] _buffs;

    protected override ICardPower InitializePower()
    {
        return new TowerBuffPower(_buffs);
    }
}
