using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/CardConfiguration", fileName = "CardConfiguration")]
public class CardConfigurationSO : ScriptableObject
{
    public CardPowerConfigurationSO cardPower;
    [SerializeField] private string _activationSoundName;
    [SerializeField] private GameObject _blueprintPrefab;
    [SerializeField] private string[] _spawnLayers;
    [SerializeField] private string _audioMixerChannel = "SoundEffects";

    public bool InGameCard;

    public string ActivationSoundName => _activationSoundName;
    public GameObject BlueprintPrefab => _blueprintPrefab;
    public string[] SpawnLayers => _spawnLayers;
    public string AudioMixerChannel => _audioMixerChannel;

    public string CardText;
    public Sprite CardSprite;
}
