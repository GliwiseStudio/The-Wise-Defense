using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBlueprint : MonoBehaviour
{
    private RaycastHit _hit;
    private CardConfigurationSO _cardConfiguration;
    private TargetDetector _spawnTargetDetector;
    private TargetDetector _updateBlueprintPositionTargetDetector;
    private bool _isInitialized = false;

    public void Initialize(CardConfigurationSO cardConfiguration)
    {
        _cardConfiguration = cardConfiguration;
        _spawnTargetDetector = new TargetDetector(_cardConfiguration.SpawnLayers);
        _updateBlueprintPositionTargetDetector = new TargetDetector("Ground");
        _isInitialized = true;
    }

    private void Update()
    {
        if(!_isInitialized)
        {
            return;
        }

        UpdatePosition();

        if(Input.GetMouseButtonDown(0))
        {
            Activate();
            Destroy(gameObject);
        }
    }

    public void Activate()
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

    private void UpdatePosition()
    {
        Vector3 newPosition = _updateBlueprintPositionTargetDetector.GetPositionFromClickInLayer();

        if (newPosition.Equals(new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity)))
        {
            return;
        }

        transform.position = newPosition;
    }
}
