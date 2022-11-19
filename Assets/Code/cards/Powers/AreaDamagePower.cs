using System.Collections.Generic;
using UnityEngine;

public class AreaDamagePower : ICardPower
{
    private readonly GameObject _prefab;
    private readonly int _damage = 30;
    private readonly float _range = 5f;
    private readonly Color _color;
    private readonly string[] _targetLayerMasks;
    private readonly TargetDetector _targetDetector;

    public AreaDamagePower(GameObject prefab, int damage, float range, Color color, string[] targetLayerMasks)
    {
        _prefab = prefab;
        _damage = damage;
        _range = range;
        _color = color;
        _targetLayerMasks = targetLayerMasks;
        _targetDetector = new TargetDetector(_range, _targetLayerMasks);
    }

    public bool Activate(GameObject gameobject, Transform transform)
    {
        if (_range < 100f)
        {
            GameObject spellSphere = GameObject.Instantiate(_prefab, transform.position, Quaternion.identity);
            spellSphere.GetComponent<SpellSphere>().SetRangeAndColor(_range, _color);
            spellSphere.SendMessage("TheStart");
        }
        
        _targetDetector.SetTransform(transform.transform);

        IReadOnlyList<Transform> objetives = _targetDetector.GetAllTargetsInRange();
        foreach (Transform t in objetives)
        {
            t.gameObject.GetComponent<IDamage>().ReceiveDamage(_damage);
        }

        return true; // this power always activates
    }
}
