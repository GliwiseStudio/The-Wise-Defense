using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsInventoryRow : MonoBehaviour
{
    [SerializeField] private InventoryCard _cardPrefab;
    private List<InventoryCard> _instantiatedCardImages;

    public void Initialize(List<CardConfigurationSO> cards, InventoryController inventoryController)
    {
        _instantiatedCardImages = new List<InventoryCard>();

        foreach (CardConfigurationSO cardConfig in cards)
        {
            InventoryCard card = SpawnImage();
            card.Initialize(cardConfig, inventoryController);
            _instantiatedCardImages.Add(card);
        }
    }

    private InventoryCard SpawnImage()
    {
        return Instantiate(_cardPrefab, this.transform);
    }
}
