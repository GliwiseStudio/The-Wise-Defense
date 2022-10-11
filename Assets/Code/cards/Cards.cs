using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    private Button _button;
    [SerializeField] private CardConfigurationSO _cardConfig;
    private TargetDetector _targetDetector;

    public void Activate(GameObject go, Transform transform)
    {
        _cardConfig.cardPower.Power.Activate(go, transform);
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _targetDetector = new TargetDetector(_cardConfig.SpawnLayers);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ActivateOnMouseClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ActivateOnMouseClick);
    }

    public void ActivateOnMouseClick()
    {
        GameObject go = _targetDetector.GetGameObjectFromClickInLayer();

        Vector3 spawnPosition = _targetDetector.GetPositionFromClickInLayer();
        GameObject newGO = new GameObject();
        newGO.transform.position = spawnPosition;

        Activate(go, newGO.transform);
    }
}
