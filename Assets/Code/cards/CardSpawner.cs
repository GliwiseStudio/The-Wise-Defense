using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private Cards _templateCardPrefab;
    [SerializeField] private CardStorageOfficial _cardsStorage;

    //public Cards Create()
    //{
    //    Cards card = _cardsStorage.GetRandomCard();
    //    return Instantiate(card);
    //}

    public Cards Create()
    {
        Cards card = Instantiate(_templateCardPrefab);
        card.SetCardConfig(_cardsStorage.GetRandomCard());
        return card;
    }

    //public Cards CreateBeforeGameCard()
    //{
    //    Cards card = _cardsStorage.GetRandomBeforeGameCard();
    //    return Instantiate(card);
    //}

    public Cards CreateBeforeGameCard()
    {
        Cards card = Instantiate(_templateCardPrefab);
        card.SetCardConfig(_cardsStorage.GetRandomBeforeGameCard());
        return card;
    }
}
