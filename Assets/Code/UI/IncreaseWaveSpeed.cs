
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]

public class IncreaseWaveSpeed : MonoBehaviour
{
    private bool _activateWaveSpeed = false;
    private Button _button;
    public GameObject textx1;
    public GameObject textx2;
    private int _speed = 1;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _speed = 1;
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
            _speed = 1;
            textx1.SetActive(true);
            textx2.SetActive(false);
        }
        else
        {
            Time.timeScale = 2;
            _speed = 2;
            textx1.SetActive(false);
            textx2.SetActive(true);
        }
    }

    public int GetSpeed()
    {
        return _speed;
    }
}
