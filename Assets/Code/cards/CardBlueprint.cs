using UnityEngine;

public class CardBlueprint : MonoBehaviour
{
    private TargetDetector _updateBlueprintPositionTargetDetector;
    private bool _isInitialized = false;

    public void Initialize()
    {
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
