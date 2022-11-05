using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaParalyzePower : ICardPower
{
    private readonly GameObject _prefab;
    private readonly float _range = 5f;
    private readonly float _duration = 3f;
    private readonly string[] _targetLayerMasks;
    private readonly TargetDetector _targetDetector;

    public AreaParalyzePower(GameObject prefab, float duration, float range, string[] targetLayerMasks)
    {
        _prefab = prefab;
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
            t.gameObject.GetComponent<IDownStats>().ReceiveTimedParalysis(_duration);
        }

        return true; // this power always activates
    }
}
