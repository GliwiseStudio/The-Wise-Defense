using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private Cards _templateCardPrefab;
    private CardStorage _cardsStorage;

    public void SetCardsStorage(CardStorage cardStorage)
    {
        _cardsStorage = cardStorage;
    }

    public Cards CreateRandomCard()
    {
        Cards card = Instantiate(_templateCardPrefab);
        card.SetCardConfig(_cardsStorage.GetRandomCard());
        return card;
    }

    public Cards CreateRandomCardFromType(CardType type, bool excludeOnlyAirTargets = false)
    {
        Cards card = Instantiate(_templateCardPrefab);
        card.SetCardConfig(_cardsStorage.GetRandomCardFromType(type, excludeOnlyAirTargets));
        return card;
    }

    public Cards CreateRandomCardExcluding(CardConfigurationSO excludedCardConfiguration)
    {
        Cards card = Instantiate(_templateCardPrefab);
        card.SetCardConfig(_cardsStorage.GetRandomCardExcluding(excludedCardConfiguration));
        return card;
    }

    public Cards GetRandomCardExcludingOnlyAirTargets()
    {
        Cards card = Instantiate(_templateCardPrefab);
        card.SetCardConfig(_cardsStorage.GetRandomCardExcludingOnlyAirTargets());
        return card;
    }
}
