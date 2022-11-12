using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
public class InventoryCard : MonoBehaviour, IPointerClickHandler
{
    private CardConfigurationSO _cardConfiguration;
    private Image _cardImage;
    private bool _isMouseOutOfTheCard = true;
    private InventoryController _inventoryController;

    public void Initialize(CardConfigurationSO cardConfiguration, InventoryController inventoryController)
    {
        _cardImage = GetComponent<Image>();
        _cardConfiguration = cardConfiguration;
        _cardImage.sprite = cardConfiguration.CardSprite;
        _inventoryController = inventoryController;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _inventoryController.VisualizeCardImage(_cardConfiguration);
    }
}
