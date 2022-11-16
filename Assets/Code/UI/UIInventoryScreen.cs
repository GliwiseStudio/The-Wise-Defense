using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventoryScreen : MonoBehaviour, UIScreen
{
    [SerializeField] private string _name;
    [SerializeField] private int _numberOfCardsPerRow = 5;
    [SerializeField] private RectTransform _inventoryCardRowsHolder;
    [SerializeField] private CardsInventoryRow _inventoryRowPrefab;
    [SerializeField] private LevelConfigurationStorage _levelConfigurationStorage;
    [SerializeField] private ScrollRect _inventoryScrollRect;
    [SerializeField] private GameObject _cardVisualizerObject;
    [SerializeField] private Image _cardVisualizerImage;
    [SerializeField] private Button _cardVisualizerGoBackButton;
    [SerializeField] private TextMeshProUGUI _cardVisualizerNameText;
    [SerializeField] private TextMeshProUGUI _cardVisualizerDescriptionText;
    private InventoryController _inventoryController;

    private void Awake()
    {
        PlayFabDataRetrieverSubstitute dataRetriever = FindObjectOfType<PlayFabDataRetrieverSubstitute>();
        _inventoryController = new InventoryController(dataRetriever, _numberOfCardsPerRow, _inventoryCardRowsHolder, _inventoryRowPrefab, _levelConfigurationStorage, _inventoryScrollRect, _cardVisualizerObject,
            _cardVisualizerImage, _cardVisualizerNameText, _cardVisualizerDescriptionText);
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public string GetName()
    {
        return _name;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _cardVisualizerGoBackButton.onClick.RemoveListener(HideInventoryCardVisualizer);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _inventoryController.Show();
        _cardVisualizerGoBackButton.onClick.AddListener(HideInventoryCardVisualizer);
    }

    private void HideInventoryCardVisualizer()
    {
        _inventoryController.HideCardVisualizer();
    }
}
