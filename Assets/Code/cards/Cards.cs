using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cards : MonoBehaviour
{
    private Button _button;
    private Image _cardImage;
    private TextMeshProUGUI _cardText;

    [SerializeField] private CardConfigurationSO _cardConfig;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _cardImage = GetComponent<Image>();
        _cardText = GetComponentInChildren<TextMeshProUGUI>();

        _cardText.SetText(_cardConfig.CardText);
        _cardImage.sprite = _cardConfig.CardSprite;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(SpawnBlueprint);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SpawnBlueprint);
    }

    private void SpawnBlueprint()
    {
        GameObject blueprint = Instantiate(_cardConfig.BlueprintPrefab);
        blueprint.GetComponent<CardBlueprint>().Initialize(_cardConfig);

        // only use it once
        _button.enabled = false;
        GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    #region Getters/setters
    public void SetCardConfig(CardConfigurationSO cardConfig)
    {
        _cardConfig = cardConfig;
        _cardText.SetText(cardConfig.CardText);
        _cardImage.sprite = cardConfig.CardSprite;
    }
    public CardConfigurationSO GetCardConfig()
    {
        return _cardConfig;
    }
    #endregion

}
