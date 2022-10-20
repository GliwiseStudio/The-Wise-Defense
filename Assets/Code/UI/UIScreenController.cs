using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UIScreenController : MonoBehaviour
{
    [SerializeField] private string _initialScreenName;
    private IDictionary<string, UIScreen> _screens;
    private UIScreen _currentScreen;
    private UIScreen _previousScreen;

    private void Awake()
    {
        _screens = new Dictionary<string, UIScreen>();
        UIScreen[] screensFound = GetComponentsInChildren<UIScreen>(true);

        foreach (UIScreen screen in screensFound)
        {
            screen.GetGameObject().SetActive(false);
            _screens.Add(screen.GetName(), screen);
        }

        SwitchScreen(_initialScreenName);
    }

    public void SwitchScreen(string screenName)
    {
        UIScreen screenToSwitch;
        bool foundScreen = _screens.TryGetValue(screenName, out screenToSwitch);

        if(!foundScreen)
        {
#if UNITY_EDITOR
            Debug.Log("Not found");
#endif
            return;
        }

        SwitchScreen(screenToSwitch);
    }

    public void SwitchToPreviousScreen()
    {
        if(_previousScreen == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("You are trying to switch to the previous screen but it is null");
#endif
            return;
        }

        SwitchScreen(_previousScreen);
    }

    private void SwitchScreen(UIScreen newScreen)
    {
        Assert.IsNotNull(newScreen, $"[UIScreenController at SwitchScreen]: The new screen is null. Aborting Switch Screen operation...");

        if(_currentScreen != null)
        {
            _currentScreen.Hide();
            _previousScreen = _currentScreen;
        }

        _currentScreen = newScreen;
        _currentScreen.Show();
    }
}
