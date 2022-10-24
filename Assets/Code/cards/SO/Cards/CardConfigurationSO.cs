using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/CardConfiguration", fileName = "CardConfiguration")]
public class CardConfigurationSO : ScriptableObject
{
    public CardPowerConfigurationSO cardPower;
    [SerializeField] private string _activationSoundName;
    [SerializeField] private AudioPlayer _audioPlayerPrefab;
    [SerializeField] private GameObject _blueprintPrefab;
    [SerializeField] private string[] _spawnLayers;

    public bool InGameCard;

    public string ActivationSoundName => _activationSoundName;
    public AudioPlayer AudioPlayerPrefab => _audioPlayerPrefab;
    public GameObject BlueprintPrefab => _blueprintPrefab;
    public string[] SpawnLayers => _spawnLayers;

    public string CardText;
    public Sprite CardSprite;
}
