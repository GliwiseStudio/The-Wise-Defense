using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DeckController : MonoBehaviour
{
    [SerializeField] private Transform _deckHolderTransform;
    [SerializeField] private int _maximumCardsInDeck = 5;
    private List<Cards> _currentCards;
    private CardSpawner _cardSpawner;
    private WaveStarter _waveStarter;

    private void Awake()
    {
        _cardSpawner = FindObjectOfType<CardSpawner>();
        _waveStarter = FindObjectOfType<WaveStarter>(true);
        _currentCards = new List<Cards>(_maximumCardsInDeck);
        GenerateDeck();
    }

    private void OnEnable()
    {
        _waveStarter.OnWaveStart += RemoveBeforeGameCards;
        GameManager.Instance.OnWaveFinished += GenerateDeck;
    }

    private void OnDisable()
    {
        _waveStarter.OnWaveStart -= RemoveBeforeGameCards;
        GameManager.Instance.OnWaveFinished -= GenerateDeck;
    }

    public void GenerateDeck()
    {
        ClearDeck();

        GenerateBeforeGameCard();
        for (int i = 1; i < _maximumCardsInDeck; i++)
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
        card.transform.SetParent(_deckHolderTransform);
        _currentCards.Add(card);
    }

    private void GenerateBeforeGameCard()
    {
        Cards card = _cardSpawner.CreateBeforeGameCard();
        card.transform.SetParent(_deckHolderTransform);
        _currentCards.Add(card);
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
}
