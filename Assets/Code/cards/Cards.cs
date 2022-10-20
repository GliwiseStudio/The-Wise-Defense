using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cards : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnSuccesfullySpawned;

    private Image _cardImage;
    private TargetDetector _spawnTargetDetector;
    private GameObject _blueprint;
    private bool _isBlueprintSpawned = false;
    private bool _isActive = false;

    [SerializeField] private CardConfigurationSO _cardConfiguration;

    private void Awake()
    {
        _cardImage = GetComponent<Image>();
        _spawnTargetDetector = new TargetDetector(_cardConfiguration.SpawnLayers);
        Activate();
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
        if (CheckIfIsAbleToActivate())
        {
            ActivatePower();
            OnSuccesfullySpawned?.Invoke();
        }
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

    private void ActivatePower()
    {
        Assert.IsNotNull(_cardConfiguration, "Can't activate this card power because there is no CardConfiguration");

        Vector3 spawnPosition = _spawnTargetDetector.GetPositionFromClickInLayer();

        if (spawnPosition.Equals(new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity)))
        {
            return;
        }

        GameObject go = _spawnTargetDetector.GetGameObjectFromClickInLayer();

        GameObject newGO = new GameObject();
        newGO.transform.position = spawnPosition;
        _cardConfiguration.cardPower.Power.Activate(go, newGO.transform);
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
        _cardImage.sprite = _cardConfiguration.CardSprite;
        _spawnTargetDetector.SetTargetLayers(_cardConfiguration.SpawnLayers);
    }

    public CardConfigurationSO GetCardConfig()
    {
        return _cardConfiguration;
    }
    #endregion

}
