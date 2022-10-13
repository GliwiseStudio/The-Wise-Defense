using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    private bool placeable = true;
    private GameObject _tower;

    public void Spawn(GameObject towerPrefab)
    {
        if(placeable)
        {
            _tower = Instantiate(towerPrefab, _spawnPoint.position, Quaternion.identity);
            placeable = false;
        }
    }

    public GameObject GetTower()
    {
        if(!placeable)
        {
            return null;
        }

        return _tower;
    }
}
