using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/CardConfiguration", fileName = "CardConfiguration")]
public class CardConfigurationSO : ScriptableObject
{
    public CardPowerConfigurationSO cardPower;
    [SerializeField] private CardType _cardType = CardType.None;
    [SerializeField] private string _activationSoundName;
    [SerializeField] private GameObject _blueprintPrefab;
    [SerializeField] private string[] _spawnLayers;
    [SerializeField] private string _audioMixerChannel = "SoundEffects";
    [SerializeField] [TextArea] private string _description;
    [SerializeField] private string _name;

    public Sprite CardSprite;


    public bool InGameCard;
    public CardType CardType => _cardType;
    public string ActivationSoundName => _activationSoundName;
    public GameObject BlueprintPrefab => _blueprintPrefab;
    public string[] SpawnLayers => _spawnLayers;
    public string AudioMixerChannel => _audioMixerChannel;
    public string CardName => _name;
    public string Description => _description;
}
