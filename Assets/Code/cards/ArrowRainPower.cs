using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Power/ArrowRainPowerConfiguration", fileName = "ArrowRainPowerConfiguration")]
public class ArrowRainPower : ICardPower
{
    private readonly float detectionRadius = 5.0f;
    private Transform detectionTransform;
    private readonly string layer = "Enemies";

    private IReadOnlyList<Transform> objectives;
    private TargetDetector _targetDetector;

    public void Activate(Transform transform)
    {
        throw new System.NotImplementedException();

        _targetDetector = new TargetDetector(transform, detectionRadius, layer);

        objectives = _targetDetector.GetAllTargetsInRange();
        foreach (Transform t in objectives)
        {
            t.gameObject.GetComponent<IDamage>().ReceiveDamage(30);
        }
    }
}
