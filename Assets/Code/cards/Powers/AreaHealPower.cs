using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaHealPower : ICardPower
{
    private readonly GameObject _prefab;
    private readonly int _health = 30;
    private readonly float _range = 5f;
    private readonly string[] _targetLayerMasks;
    private readonly TargetDetector _targetDetector;

    public AreaHealPower(GameObject prefab, int health, float range, string[] targetLayerMasks)
    {
        _prefab = prefab;
        _health = health;
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
            t.gameObject.GetComponent<IHeal>().Heal(_health);
        }
    }
}
