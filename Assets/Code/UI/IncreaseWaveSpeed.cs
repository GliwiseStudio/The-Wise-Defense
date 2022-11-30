
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]

public class IncreaseWaveSpeed : MonoBehaviour
{
    private bool _activateWaveSpeed = false;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        _button.onClick.AddListener(ClickOnButton);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ClickOnButton);
    }

    public void ClickOnButton()
    {
        if(_activateWaveSpeed == false)
        {
            _activateWaveSpeed = true;
            timeMethod();
        }

        else
        {
            _activateWaveSpeed = false;
            timeMethod();
        }
    }

    public void timeMethod()
    {
        if(_activateWaveSpeed == false)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 2;
        }
    }
}
