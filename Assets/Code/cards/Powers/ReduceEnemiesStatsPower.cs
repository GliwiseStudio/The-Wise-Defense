using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceEnemiesStatsPower : ICardPower
{
    private readonly GameObject _prefab;
    private readonly float _speedReductionPercentage;
    private readonly float _damageReductionPercentage;
    private readonly float _duration;
    private readonly float _range;
    private readonly string[] _targetLayerMasks;
    private readonly TargetDetector _targetDetector;

    public ReduceEnemiesStatsPower(GameObject prefab, float speedReductionPercentage, float damageReductionPercentage, float duration, float range, string[] targetLayerMasks)
    {
        _prefab = prefab;
        _speedReductionPercentage = speedReductionPercentage;
        _damageReductionPercentage = damageReductionPercentage;
        _duration = duration;
        _range = range;
        _targetLayerMasks = targetLayerMasks;
        _targetDetector = new TargetDetector(_range, _targetLayerMasks);
    }

    public bool Activate(GameObject gameobject, Transform transform)
    {
        GameObject.Instantiate(_prefab, transform.position, Quaternion.identity);
        _targetDetector.SetTransform(transform.transform);

        IReadOnlyList<Transform> objetives = _targetDetector.GetAllTargetsInRange();
        foreach (Transform t in objetives)
        {
            t.gameObject.GetComponent<IDownStats>().ReceiveTimedDownStats(_speedReductionPercentage, _damageReductionPercentage, _duration);
        }
        return true;
    }
}
