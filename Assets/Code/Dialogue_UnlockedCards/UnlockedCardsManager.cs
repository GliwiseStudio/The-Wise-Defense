using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedCardsManager : MonoBehaviour
{
    private Queue<CardConfigurationSO> _cards;
    [SerializeField] private Image _cardImage;
    private LevelSelection _currentLevelSelection;

    void Awake()
    {
        _cards = new Queue<CardConfigurationSO>();
        gameObject.SetActive(false);
    }

    public void ShowCards(CardConfigurationSO[] cards, LevelSelection levelSelection)
    {
        _currentLevelSelection = levelSelection;
        _cards.Clear();

        foreach (CardConfigurationSO card in cards)
        {
            _cards.Enqueue(card);
        }

        DisplayNextCard();
    }

    public void DisplayNextCard()
    {
        if (_cards.Count == 0)
        {
            EndShow();
            return;
        }

        CardConfigurationSO cardSO = _cards.Dequeue();
        _cardImage.sprite = cardSO.CardSprite;
    }

    private void EndShow()
    {
        _currentLevelSelection.GoToLevel(); // after showing the unlocked cards, go to level screen
    }
}
