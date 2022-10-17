using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    private bool _hasATower = false;
    private TowerController _tower;

    public bool HasATower => _hasATower;

    public void SpawnTower(GameObject towerPrefab)
    {
        if (_hasATower)
        {
            return;
        }

        _tower = Instantiate(towerPrefab, _spawnPoint.position, Quaternion.identity).GetComponent<TowerController>();
        _hasATower = true;
    }

    public void LevelUpTower(string towerName)
    {
        if(!_hasATower || _tower.GetName().CompareTo(towerName) != 0)
        {
            return;
        }

        _tower.LevelUp();
    }

    public GameObject GetTower()
    {
        if(_hasATower)
        {
            return null;
        }

        return _tower.gameObject;
    }
}
