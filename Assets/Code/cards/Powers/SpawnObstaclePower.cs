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

    public bool Activate(GameObject go, Transform transform)
    {
        ObstacleBase respawn = go.GetComponent<ObstacleBase>();

        if (respawn.HasAnObstacle)
        {
            return respawn.HealObstacle(_obstacleName); // if the obstacle is full health don't use card, otherwise use it and heal it
        }
        else
        {
            respawn.SpawnObstacle(_obstaclePrefab, _obstacleName);
            return true; // power activated
        }
    }

}
