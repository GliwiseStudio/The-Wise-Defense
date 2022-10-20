using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class SwitchScreenButton : MonoBehaviour
{
    [SerializeField] private string _screenNameToSwitch;
    private Button _button;
    private UIScreenController _screenController;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _screenController = FindObjectOfType<UIScreenController>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(SwitchScreen);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SwitchScreen);
    }

    private void SwitchScreen()
    {
        Assert.IsNotNull(_screenController, "[SwitchScreenButton at SwitchScreen]: The UIScreenController component is null");

        _screenController.SwitchScreen(_screenNameToSwitch);
    }
}
