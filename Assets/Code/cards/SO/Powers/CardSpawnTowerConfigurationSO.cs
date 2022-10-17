using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/CardSpawnPowerConfiguration", fileName = "CardSpawnPowerConfiguration")]
public class CardSpawnTowerConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private string _towerName;
    [SerializeField] private GameObject _towerPrefab;

    protected override ICardPower InitializePower()
    {
        return new SpawnTowerPower(_towerPrefab, _towerName);
    }

}
