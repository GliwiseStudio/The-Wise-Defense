using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Projectile/Damage Type/SingleTargetDamage", fileName = "ProjectileSingleTargetDamageConfiguration")]
public class ProjectileSingleTargetDamagerSO : ProjectileDamagerSO
{
    protected override IProjectileDamager InitializeDamager()
    {
        return new ProjectileSingleTargetDamager();
    }
}
