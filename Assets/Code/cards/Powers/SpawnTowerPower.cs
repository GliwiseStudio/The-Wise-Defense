using UnityEngine;

public class SpawnTowerPower : ICardPower
{
    private readonly GameObject _towerPrefab;

    public SpawnTowerPower(GameObject towerPrefab)
    {
        _towerPrefab = towerPrefab;
    }

    public void Activate(GameObject go, Transform transform)
    {
        TowerBase respawn = go.GetComponent<TowerBase>();
        respawn.Spawn(_towerPrefab);
    }
}
