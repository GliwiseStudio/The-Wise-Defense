using UnityEngine;

public class UnlockedCardsTrigger : MonoBehaviour
{
    [SerializeField] private UnlockedCardsManager _cardsManager;

    [SerializeField] private CardConfigurationSO[] _cards;

    public void TriggerUnlockedCards(LevelSelection levelSelection)
    {
        Debug.Log(_cardsManager);
        _cardsManager.gameObject.SetActive(true);
        _cardsManager.ShowCards(_cards, levelSelection);
    }
}
