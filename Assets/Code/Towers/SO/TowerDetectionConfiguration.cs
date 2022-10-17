using System;
using UnityEngine;

[Serializable]
public class TowerDetectionConfiguration
{
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private string[] _targetLayerMask;

    public float DetectionRange => _detectionRange;
    public string[] TargetLayerMask => _targetLayerMask;
    public void SetDetectionRange(float detectionRange)
    {
        _detectionRange = detectionRange;
    }
}
