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

    private void Initialize()
    {
        _cardImage = GetComponent<Image>();
        _deckController = FindObjectOfType<DeckController>();
        _spawnTargetDetector = new TargetDetector(_cardConfiguration.SpawnLayers);
        _spawnTargetDetector.SetTargetLayers(_cardConfiguration.SpawnLayers);
        _cardImage.sprite = _cardConfiguration.CardSprite;
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
        _blueprint = Instantiate(_cardConfiguration.BlueprintPrefab);
        _blueprint.GetComponent<CardBlueprint>().Initialize();
        _isBlueprintSpawned = true;
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
    #endregion

}
