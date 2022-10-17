using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class TowerUpgradeList
{
    [SerializeField] private List<TowerUpgrade> _upgrades;

    public TowerUpgradeList()
    {
        _upgrades = new List<TowerUpgrade>();
    }

    public TowerUpgrade GetUpgrade(int currentUpgrade)
    {
        Assert.IsFalse(currentUpgrade < 0, $"[TowerUpgradeList at GetUpgrade]: The upgrade index was {currentUpgrade}. Not valid");
        Assert.IsFalse(currentUpgrade > _upgrades.Count, $"[TowerUpgradeList at GetUpgrade]: The upgrade index was {currentUpgrade}. The maximum index is {_upgrades.Count}. Not valid");
        
        if(currentUpgrade > _upgrades.Count)
        {
            return new TowerUpgrade();
        }

        return _upgrades[currentUpgrade];
    }
}
