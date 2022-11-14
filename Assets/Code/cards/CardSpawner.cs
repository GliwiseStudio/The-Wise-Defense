using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private Cards _templateCardPrefab;
    private CardStorage _cardsStorage;

    public void SetCardsStorage(CardStorage cardStorage)
    {
        _cardsStorage = cardStorage;
    }

    public Cards Create()
    {
        Cards card = Instantiate(_templateCardPrefab);
        card.SetCardConfig(_cardsStorage.GetRandomCard());
        return card;
    }

    public Cards CreateRandomCardFromType(CardType type)
    {
        Cards card = Instantiate(_templateCardPrefab);
        card.SetCardConfig(_cardsStorage.GetRandomCardFromType(type));
        return card;
    }
}
