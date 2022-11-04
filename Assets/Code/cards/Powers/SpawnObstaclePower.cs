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
            return false; // there is an obstacle already in the base, don't activate power
        }
        else
        {
            respawn.SpawnObstacle(_obstaclePrefab);
            return true; // power activated
        }
    }

}
