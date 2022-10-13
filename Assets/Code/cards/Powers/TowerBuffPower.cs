using System.Collections.Generic;
using UnityEngine;

public class TowerBuffPower : ICardPower
{
    private readonly BuffKeyValue[] _buffs;
    public TowerBuffPower(BuffKeyValue[] buffs)
    {
        _buffs = buffs;
    }

    public void Activate(GameObject gameobject, Transform transform)
    {
        TowerBase towerBase = gameobject.GetComponent<TowerBase>();
        GameObject towerGameObject = towerBase.GetTower();

        if (towerGameObject == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("There is no tower in the selected TowerBase. Aborting Spell activation...");
#endif
            return;
        }

        towerGameObject.GetComponent<IBuff>().Buff(_buffs);
    }
}
