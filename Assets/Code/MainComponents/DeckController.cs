using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    [SerializeField] CardTypes _cardTypes;
    [SerializeField] private Transform _deckHolder;
    [SerializeField] private int _maximumCardsInDeck = 5;
    private List<Cards> _currentCards;

    private void Awake()
    {
        _currentCards = new List<Cards>(_maximumCardsInDeck);
    }

    public void GenerateDeck()
    {
        ClearDeck();
        //En caso de que ya haya cartas en el mazo, quitarlas
        GenerateCard();
        //Coger 5 cartas aleatorias
    }

    private void ClearDeck()
    {
        foreach(Cards card in _currentCards)
        {
            Destroy(card.gameObject);
        }

        _currentCards.Clear();
    }

    private void GenerateCard()
    {

    }
}
