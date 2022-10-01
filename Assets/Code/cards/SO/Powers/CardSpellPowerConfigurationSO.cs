using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/CardSpellPowerConfiguration", fileName = "CardSpellPowerConfiguration")]
public class CardSpellPowerConfigurationSO : CardPowerConfigurationSO
{
    protected override ICardPower InitializePower()
    {
        return new SpellPower();
    }
}
