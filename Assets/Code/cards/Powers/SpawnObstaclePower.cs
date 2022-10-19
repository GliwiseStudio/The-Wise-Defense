using UnityEngine;

public class SpawnObstaclePower : ICardPower
{
    private readonly GameObject _obstaclePrefab;
    private readonly string _obstacleName;

    public SpawnObstaclePower(GameObject obstaclePrefab, string obstacleName)
    {
        _obstaclePrefab = obstaclePrefab;
        _obstacleName = obstacleName;
    }

    public void Activate(GameObject go, Transform transform)
    {
        ObstacleBase respawn = go.GetComponent<ObstacleBase>();

        if (respawn.HasAObstacle)
        {
            return;
        }
        else
        {
            respawn.SpawnObstacle(_obstaclePrefab);
        }
    }
}
