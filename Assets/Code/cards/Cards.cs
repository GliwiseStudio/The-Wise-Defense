using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    private Button _button;
    [SerializeField] private CardConfigurationSO _cardConfig;

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
    }
}
