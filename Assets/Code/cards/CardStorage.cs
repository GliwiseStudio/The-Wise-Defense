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

    public CardConfigurationSO GetRandomCardFromType(CardType type)
    {
        List<CardConfigurationSO> cardsOfType = _configurations.FindAll(delegate (CardConfigurationSO card)
        {
            return (card.CardType.CompareTo(type) == 0);
        });

        int randomIndex = Random.Range(0, cardsOfType.Count);
        return cardsOfType[randomIndex];
    }

    public CardConfigurationSO GetRandomCardExcluding(CardConfigurationSO excludedCardConfiguration)
    {
        List<CardConfigurationSO> cardsOfType = _configurations.FindAll(delegate (CardConfigurationSO card)
        {
            return (card.CardName.CompareTo(excludedCardConfiguration.CardName) != 0);
        });

        int randomIndex = Random.Range(0, cardsOfType.Count);
        return cardsOfType[randomIndex];
    }
}
