using UnityEngine;

public interface IProjectileDamager
{
    public void ApplyDamage(int damage, IDamage damageableEnemy, Transform damageableEnemyTransform);
}
