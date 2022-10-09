using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTower : ICardPower
{
    placement respawn;
    public void Activate()
    {
        Debug.Log("TOWERRR");

        respawn = GameObject.FindGameObjectWithTag("TagPlacement").GetComponent<placement>();

        respawn.botonPulsado = true; //Se ha pulsado el boton (seleccionado la carta), por tanto, la variable que se encuentra en placement para regular esto, cambia

        respawn.OnMouseDown();
        respawn.OnMouseExit();
        respawn.OnMouseEnter();
    }
}
