using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/CardConfiguration", fileName = "CardConfiguration")]
public class CardConfigurationSO : ScriptableObject
{
    public CardPowerConfigurationSO cardPower;
    [SerializeField] private GameObject _blueprintPrefab;
    [SerializeField] private string[] _spawnLayers;

    public bool InGameCard;

    public GameObject BlueprintPrefab => _blueprintPrefab;
    public string[] SpawnLayers => _spawnLayers;
}
