using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Cards Storage", fileName = "CardsSotrage")]
public class CardsStorage : ScriptableObject
{
    [SerializeField] private List<Cards> _cards;

    public List<Cards> Cards => _cards;
    public Cards GetRandomCard()
    {
        int randomIndex = Random.Range(0, _cards.Count);
        return _cards[randomIndex];
    }

    public Cards GetRandomBeforeGameCard()
    {
        List<Cards> beforeGameCards =_cards.FindAll(delegate (Cards card)
        {
            return !card.GetCardConfig().InGameCard;
        });

        int randomIndex = Random.Range(0, beforeGameCards.Count);
        return beforeGameCards[randomIndex];
    }
}
