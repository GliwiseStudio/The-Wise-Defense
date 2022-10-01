using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/CardSpawnPowerConfiguration", fileName = "CardSpawnPowerConfiguration")]
public class CardSpawnTowerConfigurationSO : CardPowerConfigurationSO
{
    protected override ICardPower InitializePower()
    {
        return new SpawnTower();
    }

}
