using UnityEngine;

public class TowerBuffPower : ICardPower
{
    private readonly BuffConfiguration[] _buffConfigurations;
    public TowerBuffPower(BuffConfiguration[] buffConfigurations)
    {
        _buffConfigurations = buffConfigurations;
    }

    public bool Activate(GameObject gameobject, Transform transform)
    {
        TowerBase towerBase = gameobject.GetComponent<TowerBase>();
        GameObject towerGameObject = towerBase.GetTower();

        if (towerGameObject == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("No tower or crossbow. Aborting Spell activation...");
#endif
            return false; // there is no tower, don't activate power
        }

        bool _canActivate = true;

        BuffKeyValue[] _buffs = new BuffKeyValue[_buffConfigurations.Length];
        for (int i = 0; i < _buffConfigurations.Length; i++)
        {
            if(_buffConfigurations[i].Key == "Range" && towerBase.GetTower().GetComponent<TowerController>().GetName().CompareTo("Crossbow") == 0)
            {
                _canActivate = false; // can't activate
            }
            else
            {
                _buffs[i] = new BuffKeyValue(_buffConfigurations[i].Key, _buffConfigurations[i].BuffPercentage, _buffConfigurations[i].Duration);
                _canActivate = true;
            }
        }

        if (_canActivate)
        {
            towerGameObject.GetComponent<IBuff>().Buff(_buffs);
            return true; // power activated
        }
        else
        {
            return false;
        }
    }
}
