using UnityEngine;

public class LevelConfigurationApplier : MonoBehaviour
{
    private PlayFabDataRetrieverSubstitute _dataRetriever;
    private CardSpawner _cardSpawner;
    [SerializeField] private LevelConfigurationStorage _levelConfigurationStorage;

    private void Awake()
    {
        _dataRetriever = FindObjectOfType<PlayFabDataRetrieverSubstitute>();
        _cardSpawner = FindObjectOfType<CardSpawner>();

        InitializeCardSpawner();
    }

    private void InitializeCardSpawner()
    {
        int levelIndex = _dataRetriever.GetLastLevelIndex();
        CardStorage cardStorage = _levelConfigurationStorage.GetLevelConfigurationFromIndex(levelIndex).LevelCardStorage;
        _cardSpawner.SetCardsStorage(cardStorage);
    }
}
