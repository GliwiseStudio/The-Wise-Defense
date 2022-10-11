using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ArrowsCard : MonoBehaviour
{
    private readonly float detectionRadius = 5.0f;
    private Transform detectionTransform;
    private readonly string layer = "Enemies";

    private IReadOnlyList<Transform> objectives;
    private TargetDetector _targetDetector;

    private void Awake()
    {
        _targetDetector = new TargetDetector(this.transform, detectionRadius, layer);
    }
    
    private void Start()
    {
        objectives = _targetDetector.GetAllTargetsInRange();
        foreach(Transform t in objectives)
        {
            t.gameObject.GetComponent<IDamage>().ReceiveDamage(30);
        }
    }
}
