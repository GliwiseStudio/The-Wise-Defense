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

    #region setters
    public void SetRange(float range, Color color)
    {
        if(range < 100)
        {
            gameObject.transform.localScale = new Vector3(2 * range, 1, 2 * range); // change rage cylinder back
            Debug.Log(gameObject.GetComponent<Renderer>().material.GetColor("_Color"));
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
        }
    }
    #endregion
}
