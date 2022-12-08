using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class Cards : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    private Image _cardImage;
    private TargetDetector _spawnTargetDetector;
    private GameObject _blueprint;
    private bool _isBlueprintSpawned = false;
    private bool _isActive = false;
    private bool _isMouseOutOfTheCard = true;

    private CardConfigurationSO _cardConfiguration;
    [SerializeField] private DiscardButtonUI _discardButtonUI;
    [SerializeField] private AudioPlayer _audioPlayerPrefab;
    private DeckController _deckController;
    private CardStatsVisualizer _statsVisualizer;

    private bool _gameStarted = false; // cards can only instantiate when the game hasn't started
                                       // hence why this will be false until noticed otherwise

    private void Initialize()
    {
        _statsVisualizer = new CardStatsVisualizer(GetComponentInChildren<CardStatsHolderUI>());
        _statsVisualizer.LoadStats(_cardConfiguration.Stats);
        _cardImage = GetComponent<Image>();
        _deckController = FindObjectOfType<DeckController>();
        _spawnTargetDetector = new TargetDetector(_cardConfiguration.SpawnLayers);
        _spawnTargetDetector.SetTargetLayers(_cardConfiguration.SpawnLayers);

        _cardImage.sprite = _cardConfiguration.CardSprite;

        if (this.GetCardConfig().InGameCard) // when instantiated (before wave starts) the inGameCard has a different color
        {
            _cardImage.color = new Color32(138, 138, 138, 255);
        }

        _discardButtonUI.Hide();
        Activate();
    }

    private void OnEnable()
    {
        _discardButtonUI.OnButtonClicked += DestroyAndReplace;
    }

    private void OnDisable()
    {
        _discardButtonUI.OnButtonClicked -= DestroyAndReplace;
    }

    private void DestroyAndReplace()
    {
        _deckController.ReplaceCard(this);
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!_isActive)
        {
            return;
        }

        if (!this.GetCardConfig().InGameCard || _gameStarted)
            SpawnBlueprint();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!_isActive)
        {
            return;
        }

        DespawnBlueprint();
        if(_isMouseOutOfTheCard)
        {
            if (!this.GetCardConfig().InGameCard || _gameStarted)
            {
                if (CheckIfIsAbleToActivate())
                {
                    bool succesfullyActivated = ActivatePower();
                    if (succesfullyActivated)
                    {
                        PlayActivationSound();
                        gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            if(_deckController.IsAbleToActivateDiscardButton())
            {
                ActivateDiscardButton();
            }
        }
    }

    private void ActivateDiscardButton()
    {
        Debug.Log("Discard");
        _discardButtonUI.Show();
    }

    public void HideDiscardButton()
    {
        _discardButtonUI.Hide();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOutOfTheCard = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOutOfTheCard = false;
    }

    private void SpawnBlueprint()
    {
        if (!_isBlueprintSpawned)
        {
            _blueprint = Instantiate(_cardConfiguration.BlueprintPrefab, new Vector3(0,-100,0), Quaternion.identity);
            _blueprint.GetComponent<CardBlueprint>().Initialize();

            if (_cardConfiguration.CardType == CardType.Spell) // if its a spell, set range and color of the spell blueprint
            {
                foreach(CardStatConfiguration stat in _cardConfiguration.Stats)
                {
                    if(stat.Name == CardStatType.Range)
                    {
                        _blueprint.GetComponent<CardBlueprint>().SetSpellRange(stat.Value, _cardConfiguration.Color);
                    }
                }
            } else if(_cardConfiguration.CardType == CardType.Turret) // if its a spell, set range and color of the spell blueprint
            {
                foreach (CardStatConfiguration stat in _cardConfiguration.Stats)
                {
                    if (stat.Name == CardStatType.Range)
                    {
                        _blueprint.GetComponent<CardBlueprint>().SetTurretRange(stat.Value);
                    }
                }
            }
            _isBlueprintSpawned = true;
        }
    }

    private void DespawnBlueprint()
    {
        Destroy(_blueprint);
        _isBlueprintSpawned = false;
    }

    private bool CheckIfIsAbleToActivate()
    {
        if(_spawnTargetDetector.GetGameObjectFromClickInLayer() != null)
        {
            return true;
        }

        return false;
    }

    private bool ActivatePower()
    {
        Assert.IsNotNull(_cardConfiguration, "Can't activate this card power because there is no CardConfiguration");

        Vector3 spawnPosition = _spawnTargetDetector.GetPositionFromClickInLayer();

        if (spawnPosition.Equals(new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity)))
        {
            return false;
        }

        GameObject go = _spawnTargetDetector.GetGameObjectFromClickInLayer();

        GameObject newGO = new GameObject();
        newGO.transform.position = spawnPosition;
        bool activated = _cardConfiguration.cardPower.Power.Activate(go, newGO.transform);

        return activated;
    }

    public void PlayActivationSound()
    {
        InstantiateActivationSoundPlayer();
    }

    private void InstantiateActivationSoundPlayer()
    {
        AudioPlayer audioPlayer = Instantiate(_audioPlayerPrefab);
        audioPlayer.ConfigureAudioSource(_cardConfiguration.AudioMixerChannel);
        audioPlayer.PlayAudio(_cardConfiguration.ActivationSoundName);
    }

    private void OnDestroy()
    {
        Reset();
    }

    private void Reset()
    {
        if(_isBlueprintSpawned)
        {
            DespawnBlueprint();
        }

        _blueprint = null;
    }

    #region Getters/setters
    public void SetCardConfig(CardConfigurationSO cardConfig)
    {
        Reset();

        _cardConfiguration = cardConfig;
        Initialize();
    }

    public CardConfigurationSO GetCardConfig()
    {
        return _cardConfiguration;
    }

    public void GameStartedNotice() // to notify the in game cards that the wave has started, so they are able to do things :)
    {
        _gameStarted = true;
        _cardImage.color = new Color32(255, 255, 255, 255); // change color backk
    }
    #endregion

}
