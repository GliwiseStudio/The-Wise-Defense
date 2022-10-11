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

        //_respawn.Spawn();
        //_respawn.botonPulsado = true; //Se ha pulsado el boton (seleccionado la carta), por tanto, la variable que se encuentra en placement para regular esto, cambia

        //_respawn.OnMouseDown();
        //_respawn.OnMouseExit();
        //_respawn.OnMouseEnter();
    }
}
