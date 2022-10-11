using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/ArrowRainPowerConfiguration", fileName = "ArrowRainPowerConfiguration")]
public class ArrowRainPower : ICardPower
{
    private readonly float _detectionRadius = 5.0f;
    private Transform _detectionTransform;
    private readonly string _layer = "Enemies";

    private IReadOnlyList<Transform> _objectives;
    private TargetDetector _targetDetector;

    public void Activate(Transform transform)
    {
        Debug.Log("O");
        _targetDetector = new TargetDetector(transform, _detectionRadius, _layer);

        _objectives = _targetDetector.GetAllTargetsInRange();
        foreach (Transform t in _objectives)
        {
            t.gameObject.GetComponent<IDamage>().ReceiveDamage(30);
        }
    }
}
