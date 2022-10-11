using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/CardConfiguration", fileName = "CardConfiguration")]
public class CardConfigurationSO : ScriptableObject
{
    public CardPowerConfigurationSO cardPower;
    [SerializeField] private string[] _spawnLayers;

    public string[] SpawnLayers => _spawnLayers;
}
