using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] CardTypes _cardTypes;

    private Button _startButton;
    private Button[] _buttons;
    private Cards[] _cards;
    private Image[] _btnImages;

    private bool _waitingToContinue = false;

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>();
        _cards = GetComponentsInChildren<Cards>();
        _btnImages = GetComponentsInChildren<Image>();

        _startButton = _buttons[0];

        SetCards();
        ManageOutOfGameCards();
    }

    private void Update()
    {
        if (_waitingToContinue)
        {
            if (!GameManager.Instance.GetIsWaveActive())
            {
                SetCards();
                ManageOutOfGameCards();
                _waitingToContinue = false;
                _startButton.gameObject.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(NextWave);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(NextWave);
    }

    private void NextWave()
    {
        ManageInGameCards();
        GameManager.Instance.StartWave();
        _startButton.gameObject.SetActive(false);
        _waitingToContinue = true;
    }

    #region Manage cards

    private void SetCards()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            int pos = Random.Range(0, _cardTypes.Cards.Length);
            _cards[i].SetCardConfig(_cardTypes.Cards[pos]); // random CardConfiguration
        }
    }

    private void ManageInGameCards()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            CardConfigurationSO cardConfig = _cards[i].GetCardConfig();

            if (cardConfig.InGameCard)
            {
                _buttons[i+1].enabled = true;
                _btnImages[i+1].color = new Color(1f, 1f, 1f, 1f);
                //_buttons[i + 1].gameObject.SetActive(true);
            }
            else
            {
                _buttons[i + 1].enabled = false;
                _btnImages[i + 1].color = new Color(0.5f, 0.5f, 0.5f, 1f);
                //_buttons[i + 1].gameObject.SetActive(false);
            }
        }
    }
    private void ManageOutOfGameCards()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            CardConfigurationSO cardConfig = _cards[i].GetCardConfig();

            if (!cardConfig.InGameCard)
            {
                _buttons[i + 1].enabled = true;
                _btnImages[i + 1].color = new Color(1f, 1f, 1f, 1f);
                //_buttons[i + 1].gameObject.SetActive(true);
            }
            else
            {
                _buttons[i + 1].enabled = false;
                _btnImages[i + 1].color = new Color(0.5f, 0.5f, 0.5f, 1f);
                //_buttons[i + 1].gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
