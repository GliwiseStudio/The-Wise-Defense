using UnityEngine;

public class ProjectileSingleTargetDamager : IProjectileDamager
{
    public ProjectileSingleTargetDamager() { }

    public void ApplyDamage(int damage, IDamage damageableEnemy, Transform damageableEnemyTransform)
    {
        damageableEnemy.ReceiveDamage(damage);
    }
}
