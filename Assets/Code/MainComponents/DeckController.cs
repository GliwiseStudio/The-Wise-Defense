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
    [SerializeField] private int _minimumTurretCardsInDeckGenerator = 1;
    [SerializeField] private int _maximumDiscardsPerRound = 2;

    private int _currentDiscards;
    private bool _isAbleToDiscardCards = true;
    private List<Cards> _currentCards;
    private CardSpawner _cardSpawner;

    private void Awake()
    {
        _cardSpawner = FindObjectOfType<CardSpawner>();
        _currentCards = new List<Cards>(_maximumCardsInDeck);
    }

    private void Start()
    {
        GenerateDeck();
    }

    private void OnEnable()
    {
        SubscribeToOnWaveStartedEvents();
        SubscribeToOnWaveFinishedEvents();
    }

    private void OnDisable()
    {
        UnsubscribeToOnWaveStartedEvents();
        UnsubscribeToOnWaveFinishedEvents();
    }

    private void SubscribeToOnWaveStartedEvents()
    {
        GameManager.Instance.OnWaveStarted += RemoveBeforeGameCards;
        GameManager.Instance.OnWaveStarted += DisableDiscardCards;
    }

    private void UnsubscribeToOnWaveStartedEvents()
    {
        GameManager.Instance.OnWaveStarted -= RemoveBeforeGameCards;
        GameManager.Instance.OnWaveStarted -= DisableDiscardCards;
    }

    private void SubscribeToOnWaveFinishedEvents()
    {
        GameManager.Instance.OnWaveFinished += GenerateDeck;
        GameManager.Instance.OnWaveFinished += EnableDiscardCards;
    }

    private void UnsubscribeToOnWaveFinishedEvents()
    {
        GameManager.Instance.OnWaveFinished -= GenerateDeck;
        GameManager.Instance.OnWaveFinished -= EnableDiscardCards;
    }

    public void GenerateDeck()
    {
        _currentDiscards = _maximumDiscardsPerRound;
        SetDiscardText();

        ClearDeck();

        GenerateObligatoryTurretCards();
        for (int i = _minimumTurretCardsInDeckGenerator; i < _maximumCardsInDeck; i++)
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

    private void GenerateObligatoryTurretCards()
    {
        if (_minimumTurretCardsInDeckGenerator > _maximumCardsInDeck)
        {
            _minimumTurretCardsInDeckGenerator = _maximumCardsInDeck;
#if UNITY_EDITOR
            Debug.LogWarning($"[DeckController at GenerateObligatoryTurretCards]: The minimum Turret cards number is higher than the maximum cards in deck ({_minimumTurretCardsInDeckGenerator} > {_maximumCardsInDeck}). Correct it. Using 0 so far");
#endif
            _minimumTurretCardsInDeckGenerator = 0;
            return;
        }
        for (int i = 0; i < _minimumTurretCardsInDeckGenerator; i++)
        {
            Cards card = _cardSpawner.CreateRandomCardFromType(CardType.Turret);
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
            else
            {
                _currentCards[i].GameStartedNotice();
            }
        }

        DeactivateDiscardButton(); // hide the discard button if it was still active, because once the wave is started, the player cannot discard
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

    private void EnableDiscardCards()
    {
        _isAbleToDiscardCards = true;
    }

    private void DisableDiscardCards()
    {
        _isAbleToDiscardCards = false;
    }

    public bool IsAbleToActivateDiscardButton()
    {
        if (_currentDiscards == 0 || !_isAbleToDiscardCards)
        {
            return false;
        }

        DeactivateDiscardButton();

        return true;
    }

    public void DeactivateDiscardButton()
    {
        foreach (Cards card in _currentCards)
        {
            card.HideDiscardButton();
        }
    }

    private void SetDiscardText()
    {
        _discardPerRoundTextComponent.text = _currentDiscards.ToString();
    }
}
