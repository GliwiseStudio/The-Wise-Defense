using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class InventoryController
{
    private readonly CardsInventoryRow _rowPrefab;
    private readonly List<CardsInventoryRow> _instantiatedRows;
    private readonly RectTransform _cardRowsHolder;
    private readonly PlayFabDataRetrieverSubstitute _dataRetriever;
    private int _lastLevelIndex = -1;
    private readonly int _numberOfCardsPerRow;
    private readonly LevelConfigurationStorage _levelConfigurationsStorage;
    private readonly ScrollRect _scrollRect;
    private readonly InventoryCard _cardInventoryVisualizer;
    private readonly GameObject _cardVisualizerObject;
    private readonly Image _visualizerCardImage;
    private readonly TextMeshProUGUI _visualizerCardNameText;
    private readonly TextMeshProUGUI _visualizerCardDescriptionText;

    public InventoryController(PlayFabDataRetrieverSubstitute dataRetriever, int numberOfCardsPerRow, RectTransform cardRowsHolder, CardsInventoryRow rowPrefab, LevelConfigurationStorage levelConfigurationsStorage,
        ScrollRect scrollRect, InventoryCard cardInventoryVisualizer, GameObject cardVisualizerObject, Image visualizerCardImage, TextMeshProUGUI visualizerCardNameText, TextMeshProUGUI visualizerCardDescriptionText)
    {
        _dataRetriever = dataRetriever;
        _numberOfCardsPerRow = numberOfCardsPerRow;
        _cardRowsHolder = cardRowsHolder;
        _rowPrefab = rowPrefab;
        _levelConfigurationsStorage = levelConfigurationsStorage;
        _scrollRect = scrollRect;
        _cardInventoryVisualizer = cardInventoryVisualizer;
        _cardVisualizerObject = cardVisualizerObject;
        _visualizerCardImage = visualizerCardImage;
        _visualizerCardNameText = visualizerCardNameText;
        _visualizerCardDescriptionText = visualizerCardDescriptionText;
        _instantiatedRows = new List<CardsInventoryRow>();
    }

    public void Show()
    {
        HideCardVisualizer();

        int lastLevelIndex = _dataRetriever.GetLastLevelIndex();
        ResetScrollRect();

        if (_lastLevelIndex != lastLevelIndex)
        {
            _lastLevelIndex = lastLevelIndex;
            ClearInventory();
            GenerateInventory();
        }
    }

    private void ResetScrollRect()
    {
        _scrollRect.verticalNormalizedPosition = 1f;
    }

    private void ClearInventory()
    {
        if(_instantiatedRows.Count == 0)
        {
            return;
        }

        foreach(CardsInventoryRow row in _instantiatedRows)
        {
            GameObject.Destroy(row.gameObject);
        }

        _instantiatedRows.Clear();
    }

    private void GenerateInventory()
    {
        Queue<CardConfigurationSO> inventoryCards = new Queue<CardConfigurationSO>(GetInventoryCards());
        List<CardConfigurationSO> cardsInRow = new List<CardConfigurationSO>();
        int i = 0;

        while (inventoryCards.Count > 0)
        {
            while(inventoryCards.Count > 0 && i < _numberOfCardsPerRow)
            {
                cardsInRow.Add(inventoryCards.Dequeue());
                i++;
            }

            CardsInventoryRow row = SpawnRow();
            row.Initialize(cardsInRow, this);
            _instantiatedRows.Add(row);

            cardsInRow.Clear();
            i = 0;
        }
    }

    private List<CardConfigurationSO> GetInventoryCards()
    {
        return _levelConfigurationsStorage.GetLevelConfigurationFromIndex(_lastLevelIndex).LevelCardStorage.Cards;
    }

    private CardsInventoryRow SpawnRow()
    {
        return GameObject.Instantiate(_rowPrefab, _cardRowsHolder);
    }

    public void VisualizeCardImage(CardConfigurationSO configuration)
    {
        _visualizerCardImage.sprite = configuration.CardSprite;
        _visualizerCardNameText.text = configuration.CardName;
        _visualizerCardDescriptionText.text = configuration.Description;
        ShowCardVisualizer();
        _cardInventoryVisualizer.Initialize(configuration, this);
    }

    private void ShowCardVisualizer()
    {
        _cardVisualizerObject.SetActive(true);
    }

    public void HideCardVisualizer()
    {
        _cardVisualizerObject.SetActive(false);

    }
}
