using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceEnemiesStatsPower : ICardPower
{
    private readonly GameObject _prefab;
    private readonly int _speed = 30;
    private readonly int _damage = 30;
    private readonly float _range = 5f;
    private readonly string[] _targetLayerMasks;
    private readonly TargetDetector _targetDetector;

    public ReduceEnemiesStatsPower(GameObject prefab, int speed, int damage, float range, string[] targetLayerMasks)
    {
        _prefab = prefab;
        _speed = speed;
        _damage = damage;
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
            //Here comes the method for reduce stats of enemies. This is for damage -->
            //t.gameObject.GetComponent<IDamage>().ReceiveDamage(_damage);
        }
    }
}
