using UnityEngine;

public class SpawnTower : ICardPower
{
    private readonly GameObject _towerPrefab;
    private string[] _spawnLayers;
    private TargetDetector _targetDetector;

    public SpawnTower(GameObject towerPrefab)
    {
        _towerPrefab = towerPrefab;
        _targetDetector = new TargetDetector(_spawnLayers);
    }

    public void Activate(GameObject go, Transform transform)
    {
        TowerBase respawn = go.GetComponent<TowerBase>();
        respawn.Spawn(_towerPrefab);
    }
}
