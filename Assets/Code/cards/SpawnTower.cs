using UnityEngine;

public class SpawnTower : ICardPower
{
    private placement _respawn;

    public void Activate(Transform transform)
    {
        _respawn = GameObject.FindGameObjectWithTag("TagPlacement").GetComponent<placement>();

        _respawn.botonPulsado = true; //Se ha pulsado el boton (seleccionado la carta), por tanto, la variable que se encuentra en placement para regular esto, cambia

        _respawn.OnMouseDown();
        _respawn.OnMouseExit();
        _respawn.OnMouseEnter();
    }
}
