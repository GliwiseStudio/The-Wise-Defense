public class ProjectileSingleTargetDamager : IProjectileDamager
{
    public ProjectileSingleTargetDamager() { }

    public void ApplyDamage(int damage, IDamage damageableEnemy)
    {
        damageableEnemy.ReceiveDamage(damage);
    }
}
