using UnityEngine;

public class SpawnTowerPower : ICardPower
{
    private readonly GameObject _towerPrefab;
    private readonly string _towerName;

    public SpawnTowerPower(GameObject towerPrefab, string towerName)
    {
        _towerPrefab = towerPrefab;
        _towerName = towerName;
    }

    public bool Activate(GameObject go, Transform transform)
    {
        TowerBase respawn = go.GetComponent<TowerBase>();

        if(respawn.HasATower)
        {
            bool leveledUp = respawn.LevelUpTower(_towerName);
            return leveledUp;
        }
        else
        {
            respawn.SpawnTower(_towerPrefab);
            return true; // power activated
        }
    }
}
