using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class CardStatUI : MonoBehaviour
{
    [SerializeField] private Image _imageComponent;
    [SerializeField] private TextMeshProUGUI _textComponent;

    public void SetValue(int value)
    {
        _textComponent.text = value.ToString();
    }

    public void SetValue(float value)
    {
        _textComponent.text = value.ToString();
    }
}
