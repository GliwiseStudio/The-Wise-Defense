using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockedCardsManager : MonoBehaviour
{
    private Queue<CardConfigurationSO> _cards;
    [SerializeField] private Image _cardImage;
    [SerializeField] private TextMeshProUGUI _cardTitleText;
    [SerializeField] private TextMeshProUGUI _cardDescriptionText;
    private LevelSelection _currentLevelSelection;

    private CardStatsVisualizer _cardStatsVisualizer;
    [SerializeField] private CardStatsHolderUI _holderUI;

    void Awake()
    {
        _cards = new Queue<CardConfigurationSO>();
        gameObject.SetActive(false);

        _cardStatsVisualizer = new CardStatsVisualizer(_holderUI);
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
        _cardStatsVisualizer.LoadStats(cardSO.Stats);
        _cardTitleText.text = cardSO.CardName;
        _cardDescriptionText.text = cardSO.Description;
    }

    private void EndShow()
    {
        gameObject.SetActive(false);
        _currentLevelSelection.ShowEnemiesNext(); // after showing the unlocked cards, show the unlocked enemies
    }
}
