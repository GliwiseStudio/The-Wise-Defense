using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    private bool placeable = true;

    public void Spawn(GameObject towerPrefab)
    {
        if(placeable)
        {
            Instantiate(towerPrefab, _spawnPoint.position, Quaternion.identity);
            placeable = false;
        }
    }
}
