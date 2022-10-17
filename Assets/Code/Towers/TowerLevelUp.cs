using System;

public class TowerLevelUp
{
    public event Action<TowerUpgrade> OnLevelUp;

    private int _currentLevel = 0;
    private readonly TowerUpgradeList _upgradeList;

    public TowerLevelUp(TowerUpgradeList upgradeList)
    {
        _upgradeList = upgradeList;
    }

    public void LevelUp()
    {
        OnLevelUp?.Invoke(_upgradeList.GetUpgrade(_currentLevel));
        _currentLevel++;
    }
}
