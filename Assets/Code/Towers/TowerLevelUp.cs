using System;
using UnityEngine;

public class TowerLevelUp
{
    public event Action<TowerUpgrade> OnLevelUp;

    private int _currentLevel = 0;
    private readonly TowerUpgradeList _upgradeList;

    public int MaximumLevel => _upgradeList.MaximumLevel;

    public bool IsInMaximumLevel()
    {
        if(_currentLevel == _upgradeList.MaximumLevel)
        {
            return true;
        }

        return false;
    }

    public TowerLevelUp(TowerUpgradeList upgradeList)
    {
        _upgradeList = upgradeList;
    }

    public void LevelUp()
    {
        if(IsInMaximumLevel())
        {
#if UNITY_EDITOR
            Debug.Log("You can not upgrade more this tower. It is already in maximum level");
#endif
            return;
        }

        OnLevelUp?.Invoke(_upgradeList.GetUpgrade(_currentLevel));
        _currentLevel++;
    }
}
