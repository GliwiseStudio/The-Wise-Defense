using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaParalyzePower : ICardPower
{
    private readonly GameObject _prefab;
    private readonly int _speed = 30;
    private readonly float _range = 5f;
    private readonly string[] _targetLayerMasks;
    private readonly TargetDetector _targetDetector;

    public AreaParalyzePower(GameObject prefab, int speed, float range, string[] targetLayerMasks)
    {
        _prefab = prefab;
        _speed = speed;
        _range = range;
        _targetLayerMasks = targetLayerMasks;
        _targetDetector = new TargetDetector(_range, _targetLayerMasks);
    }

    public void Activate(GameObject gameobject, Transform transform)
    {
        GameObject.Instantiate(_prefab, transform.position, Quaternion.identity);
        _targetDetector.SetTransform(transform.transform);

        IReadOnlyList<Transform> objetives = _targetDetector.GetAllTargetsInRange();
        foreach (Transform t in objetives)
        {
            //Here comes the method for slowdown enemies. This is for damage -->
            //t.gameObject.GetComponent<IDamage>().ReceiveDamage(_damage);
        }
    }
}
