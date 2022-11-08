using System;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform), typeof(Button))]
public class DiscardButtonUI : MonoBehaviour
{
    public event Action OnButtonClicked;
    private RectTransform _rectTransform;
    private Button _buttonComponent;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _buttonComponent = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _buttonComponent.onClick.AddListener(ClickButton);
    }

    private void OnDisable()
    {
        _buttonComponent.onClick.RemoveListener(ClickButton);
    }

    private void ClickButton()
    {
        OnButtonClicked?.Invoke();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
