using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/CardSpawnPowerConfiguration", fileName = "CardSpawnPowerConfiguration")]
public class CardSpawnTowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private GameObject _towerPrefab;

    protected override ICardPower InitializePower()
    {
        return new SpawnTower(_towerPrefab);
    }

}
