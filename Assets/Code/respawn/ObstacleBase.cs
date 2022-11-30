using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    private bool _hasAnObstacle = false;
    private GameObject _obstacle;
    private string _obstacleName;

    public bool HasAnObstacle => _hasAnObstacle;

    public void SpawnObstacle(GameObject obstaclePrefab, string obstacleName)
    {
        if (_hasAnObstacle)
        {
            return;
        }

        _obstacle = Instantiate(obstaclePrefab, _spawnPoint.position, transform.rotation);
        _obstacleName = obstacleName;

        _hasAnObstacle = true;
    }

    public bool HealObstacle(string obstacleName)
    {
        if (obstacleName != _obstacleName)
        {
            return false;
        }
        else
        {
            return _obstacle.GetComponent<IHeal>().FullHeal();
        }
    }

    public void Update()
    {
        if (_obstacle == null)
        {
            _hasAnObstacle = false;
        }
    }
}
