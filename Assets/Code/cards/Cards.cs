using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    private Button _button;
    public CardConfigurationSO cardConfig;
    private TargetDetector _targetDetector;

    public void Activate(Transform transform)
    {
        cardConfig.cardPower.Power.Activate(transform);
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _targetDetector = new TargetDetector("Ground");
    }

    private Transform GetTransform()
    {
        return transform;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ActivateOnMouseClick);
        //Transform tr = GetTransform();
        //button.onClick.AddListener(Activate(tr));
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ActivateOnMouseClick);
    }

    public void ActivateOnMouseClick()
    {
        Vector3 spawnPosition = _targetDetector.GetPositionFromClickInLayer();
        GameObject newGO = new GameObject();
        newGO.transform.position = spawnPosition;

        Debug.Log("H");

        Activate(newGO.transform);
    }
}
