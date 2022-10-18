using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Cards : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    private Button _button;
    private Image _cardImage;
    private TextMeshProUGUI _cardText;
    private TargetDetector _spawnTargetDetector;
    private TargetDetector _updateBlueprintPositionTargetDetector;
    private GameObject _blueprint;
    private bool _isBlueprintSpawned;

    [SerializeField] private CardConfigurationSO _cardConfiguration;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _cardImage = GetComponent<Image>();
        _cardText = GetComponentInChildren<TextMeshProUGUI>();

        //_cardText.SetText(_cardConfiguration.CardText);
        //_cardImage.sprite = _cardConfiguration.CardSprite;

        _spawnTargetDetector = new TargetDetector();//TODO PASARLE LAS CAPAS DE LA CONFIGURACIÓN
        _updateBlueprintPositionTargetDetector = new TargetDetector("Ground");
    }

    private void OnEnable()
    {
        //_button.onClick.AddListener(SpawnBlueprint);
    }

    private void OnDisable()
    {
        //_button.onClick.RemoveListener(SpawnBlueprint);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SpawnBlueprint2();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("dd");
        DespawnBlueprint();
        if (CheckIfIsAbleToActivate())
        {
            Activate();
        }
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        UpdateBlueprintPosition();
    }

    private void SpawnBlueprint()
    {
        GameObject blueprint = Instantiate(_cardConfiguration.BlueprintPrefab);
        blueprint.GetComponent<CardBlueprint>().Initialize(_cardConfiguration);

        // only use it once
        _button.enabled = false;
        GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    private void SpawnBlueprint2()
    {
        _blueprint = Instantiate(_cardConfiguration.BlueprintPrefab);
        _isBlueprintSpawned = true;
    }

    private void DespawnBlueprint()
    {
        Destroy(_blueprint);
        _isBlueprintSpawned = false;
    }

    private bool CheckIfIsAbleToActivate()
    {
        return false;
    }

    private void Activate()
    {
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

    private void UpdateBlueprintPosition()
    {
        if (!_isBlueprintSpawned)
        {
            return;
        }

        Vector3 newPosition = _updateBlueprintPositionTargetDetector.GetPositionFromClickInLayer();

        if (newPosition.Equals(new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity)))
        {
            return;
        }

        _blueprint.transform.position = newPosition;
    }

    #region Getters/setters
    public void SetCardConfig(CardConfigurationSO cardConfig)
    {
        _cardConfiguration = cardConfig;
        _cardText.SetText(cardConfig.CardText);
        _cardImage.sprite = cardConfig.CardSprite;
        _spawnTargetDetector.SetTargetLayers(cardConfig.SpawnLayers);
    }
    public CardConfigurationSO GetCardConfig()
    {
        return _cardConfiguration;
    }
    #endregion

}
