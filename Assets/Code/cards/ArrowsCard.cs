using System.Collections.Generic;
using UnityEngine;

public class ArrowsCard : MonoBehaviour
{
    private readonly float _detectionRadius = 5.0f;
    private Transform _detectionTransform;
    private readonly string _layer = "Enemies";

    private IReadOnlyList<Transform> _objectives;
    private TargetDetector _targetDetector;

    private void Awake()
    {
        _targetDetector = new TargetDetector(this.transform, _detectionRadius, _layer);
    }
    
    private void Start()
    {
        _objectives = _targetDetector.GetAllTargetsInRange();
        foreach(Transform t in _objectives)
        {
            t.gameObject.GetComponent<IDamage>().ReceiveDamage(30);
        }
    }
}
