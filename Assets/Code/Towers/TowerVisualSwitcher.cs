using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TowerVisualSwitcher
{
    private readonly List<GameObject> _visuals;
    private GameObject _currentVisuals;

    private readonly AnimationsHandler _animationsHandler;

    public TowerVisualSwitcher(List<GameObject> upgradeVisuals, AnimationsHandler animationsHandler)
    {
        _animationsHandler = animationsHandler;
        _visuals = upgradeVisuals;

        foreach(GameObject obj in _visuals)
        {
            obj.SetActive(false);
        }

        SwitchVisuals(1);
    }

    public void SwitchVisuals(int level)
    {
        int listIndex = level - 1;
        Assert.IsTrue(listIndex < _visuals.Count && listIndex >= 0);

        if(_currentVisuals != null)
        {
            _currentVisuals.SetActive(false);
        }
        _currentVisuals = _visuals[listIndex];
        _animationsHandler.SetAnimator(_currentVisuals.GetComponent<Animator>());
        _currentVisuals.SetActive(true);
    }
}
