using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevelUp
{
    private int _currentLevel = 1;
    private readonly TowerUpgradeList _upgradeList;

    public TowerLevelUp(TowerUpgradeList upgradeList)
    {
        _upgradeList = upgradeList;
    }

    public void Upgrade()
    {
        _currentLevel++;

        ApplyUpgrade(_upgradeList.GetUpgrade(_currentLevel));
    }

    private void ApplyUpgrade(TowerUpgrade newUpgrade)
    {

    }
}
