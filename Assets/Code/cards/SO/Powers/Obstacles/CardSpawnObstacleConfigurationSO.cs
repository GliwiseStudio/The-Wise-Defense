using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/CardSpawnPowerObstacleConfiguration", fileName = "CardSpawnPowerObstacleConfiguration")]
public class CardSpawnObstacleConfigurationSO : CardPowerConfigurationSO
{
    [SerializeField] private string _obstacleName;
    [SerializeField] private GameObject _obstaclePrefab;

    protected override ICardPower InitializePower()
    {
        return new SpawnObstaclePower(_obstaclePrefab, _obstacleName);
    }
}
