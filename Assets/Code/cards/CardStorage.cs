using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Cards Storage", fileName = "CardsSotrage")]
public class CardStorage : ScriptableObject
{
    [SerializeField] private List<CardConfigurationSO> _configurations;

    public List<CardConfigurationSO> Cards => _configurations;
    public CardConfigurationSO GetRandomCard()
    {
        int randomIndex = Random.Range(0, _configurations.Count);
        return _configurations[randomIndex];
    }

    public CardConfigurationSO GetRandomBeforeGameCard()
    {
        List<CardConfigurationSO> beforeGameCards = _configurations.FindAll(delegate (CardConfigurationSO card)
        {
            return !card.InGameCard;
        });

        int randomIndex = Random.Range(0, beforeGameCards.Count);
        return beforeGameCards[randomIndex];
    }
}
