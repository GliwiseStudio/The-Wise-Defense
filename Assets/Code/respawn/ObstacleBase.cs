using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    private bool _hasAObstacle = false;

    public bool HasAObstacle => _hasAObstacle;

    public void SpawnObstacle(GameObject obstaclePrefab)
    {
        if (_hasAObstacle)
        {
            return;
        }

         Instantiate(obstaclePrefab, _spawnPoint.position, transform.rotation);
        _hasAObstacle = true;
    }
}
