using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class CardStatUI : MonoBehaviour
{
    [SerializeField] private Image _imageComponent;
    [SerializeField] private TextMeshProUGUI _textComponent;

    public void SetValue(string value)
    {
        _textComponent.text = value;
    }

    public void SetSprite(Sprite sprite)
    {
        _imageComponent.sprite = sprite;
    }
}
