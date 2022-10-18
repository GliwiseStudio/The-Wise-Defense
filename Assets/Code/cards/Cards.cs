using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    [SerializeField] CardTypes _cardTypes;
    private Button _button;
    private CardConfigurationSO _cardConfig;

    private void Awake()
    {
        _button = GetComponent<Button>();
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
    }
    public CardConfigurationSO GetCardConfig()
    {
        return _cardConfig;
    }
    #endregion
}
