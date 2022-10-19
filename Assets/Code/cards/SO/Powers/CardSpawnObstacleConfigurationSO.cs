using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/CardSpawnPowerObstacleConfiguration", fileName = "CardSpawnPowerObstacleConfiguration")]
public class CardSpawnObstacleConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private string _obstacleName;
    [SerializeField] private GameObject _obstaclePrefab;

    protected override ICardPower InitializePower()
    {
        return new SpawnTowerPower(_obstaclePrefab, _obstacleName);
    }
}
