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
    private CardStatsVisualizer _statsVisualizer;
    [SerializeField] private CardStatsHolderUI _statsUIHolder;

    private void Awake()
    {
        _statsVisualizer = new CardStatsVisualizer(_statsUIHolder);
    }

    public void Initialize(CardConfigurationSO cardConfiguration, InventoryController inventoryController)
    {
        _cardImage = GetComponent<Image>();
        _cardConfiguration = cardConfiguration;
        _cardImage.sprite = cardConfiguration.CardSprite;
        _inventoryController = inventoryController;
        _statsVisualizer.LoadStats(cardConfiguration.Stats);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _inventoryController.VisualizeCardImage(_cardConfiguration);
    }
}
