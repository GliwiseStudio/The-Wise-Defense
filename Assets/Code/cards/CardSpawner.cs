using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private CardsStorage _cardsStorage;

    public Cards Create()
    {
        Cards card = _cardsStorage.GetRandomCard();
        return Instantiate(card);
    }

    public Cards CreateBeforeGameCard()
    {
        Cards card = _cardsStorage.GetRandomBeforeGameCard();
        return Instantiate(card);
    }
}
