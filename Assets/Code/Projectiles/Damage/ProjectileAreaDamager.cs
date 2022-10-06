using System.Collections.Generic;
using UnityEngine;

public class ProjectileAreaDamager : IProjectileDamager
{
    private readonly float _radius;
    private readonly string _targerLayerMask;
    private readonly TargetDetector _targetDetector;

    public ProjectileAreaDamager(float radius, string targetLayerMask)
    {
        _radius = radius;
        _targerLayerMask = targetLayerMask;
        _targetDetector = new TargetDetector(_radius, _targerLayerMask);
    }

    public void ApplyDamage(int damage, IDamage damageableEnemy, Transform damageableEnemyTransform)
    {
        _targetDetector.SetTransform(damageableEnemyTransform);
        IReadOnlyList<Transform> enemiesInRange =_targetDetector.GetAllTargetsInRange();

        foreach(Transform enemy in enemiesInRange)
        {
            IDamage damageComponent = enemy.GetComponent<IDamage>();
            damageComponent.ReceiveDamage(damage);

        }
    }
}
