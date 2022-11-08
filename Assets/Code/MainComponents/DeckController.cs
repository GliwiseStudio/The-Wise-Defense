using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

[DisallowMultipleComponent]
public class DeckController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _discardPerRoundTextComponent;
    [SerializeField] private Transform _deckHolderTransform;
    [SerializeField] private int _maximumCardsInDeck = 5;
    [SerializeField] private int _minimumBeforeCardsInDeckGenerator = 1;
    [SerializeField] private int _maximumDiscardsPerRound = 2;
    private int _currentDiscards;
    private List<Cards> _currentCards;
    private CardSpawner _cardSpawner;

    private void Awake()
    {
        _cardSpawner = FindObjectOfType<CardSpawner>();
        _currentCards = new List<Cards>(_maximumCardsInDeck);
        GenerateDeck();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnWaveStarted += RemoveBeforeGameCards;
        GameManager.Instance.OnWaveFinished += GenerateDeck;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnWaveStarted -= RemoveBeforeGameCards;
        GameManager.Instance.OnWaveFinished -= GenerateDeck;
    }

    public void GenerateDeck()
    {
        _currentDiscards = _maximumDiscardsPerRound;
        SetDiscardText();

        ClearDeck();

        GenerateBeforeGameCard();
        for (int i = _minimumBeforeCardsInDeckGenerator; i < _maximumCardsInDeck; i++)
        {
            GenerateCard();
        }
    }

    private void ClearDeck()
    {
        if(_currentCards.Count == 0)
        {
            return;
        }

        for (int i = (_currentCards.Count - 1); i >= 0; i--)
        {
            RemoveCard(i);
        }
    }

    private void GenerateCard()
    {
        Cards card = _cardSpawner.Create();
        card.transform.SetParent(_deckHolderTransform, false);
        _currentCards.Add(card);
    }

    private void GenerateBeforeGameCard()
    {
        if (_minimumBeforeCardsInDeckGenerator > _maximumCardsInDeck)
        {
            _minimumBeforeCardsInDeckGenerator = _maximumCardsInDeck;
#if UNITY_EDITOR
            Debug.LogWarning($"[DeckController at GenerateBeforeGameCard]: The minimum Before-Game cards number is higher than the maximum cards in deck ({_minimumBeforeCardsInDeckGenerator} > {_maximumCardsInDeck}). Correct it. Using 0 so far");
#endif
            _minimumBeforeCardsInDeckGenerator = 0;
            return;
        }
        for (int i = 0; i < _minimumBeforeCardsInDeckGenerator; i++)
        {
            Debug.Log("Before game card");
            Cards card = _cardSpawner.CreateBeforeGameCard();
            card.transform.SetParent(_deckHolderTransform, false);
            _currentCards.Add(card);
        }
    }

    private void RemoveBeforeGameCards()
    {
        if(_currentCards.Count == 0)
        {
            return;
        }

        for (int i = (_currentCards.Count - 1); i >= 0; i--)
        {
            if(IsBeforeGameCard(_currentCards[i]))
            {
                RemoveCard(i);
            }
        }
    }

    private bool IsBeforeGameCard(Cards card)
    {
        return !card.GetCardConfig().InGameCard;
    }

    private void RemoveCard(int cardIndex)
    {
        if(cardIndex < 0 || cardIndex >= _currentCards.Count)
        {
            return;
        }

        Destroy(_currentCards[cardIndex].gameObject);
        _currentCards.RemoveAt(cardIndex);
    }

    public void ReplaceCard(Cards cardToDelete)
    {
        Assert.IsTrue(_currentCards.Contains(cardToDelete), "[DeckController at ReplaceCard]: The card you want to replace does not exist in the deck cards list. Aborting replace operation...");
        _currentDiscards--;
        SetDiscardText();
        Destroy(cardToDelete.gameObject);
        _currentCards.Remove(cardToDelete);
        GenerateCard();
    }

    public bool IsAbleToActivateDiscardButton()
    {
        if(_currentDiscards == 0)
        {
            return false;
        }

        foreach (Cards card in _currentCards)
        {
            card.HideDiscardButton();
        }

        return true;
    }

    private void SetDiscardText()
    {
        _discardPerRoundTextComponent.text = _currentDiscards.ToString();
    }
}
