using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    private bool _hasAnObstacle = false;
    private GameObject _obstacle;

    public bool HasAnObstacle => _hasAnObstacle;

    public void SpawnObstacle(GameObject obstaclePrefab)
    {
        if (_hasAnObstacle)
        {
            return;
        }

        _obstacle = Instantiate(obstaclePrefab, _spawnPoint.position, transform.rotation);
        _hasAnObstacle = true;
    }

    public void Update()
    {
        if (_obstacle == null)
        {
            _hasAnObstacle = false;
        }
    }
}
