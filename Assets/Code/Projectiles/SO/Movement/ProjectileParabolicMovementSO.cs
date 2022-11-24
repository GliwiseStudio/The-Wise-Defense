using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Projectile/Movement Type/ParabolicMovement", fileName = "ProjectileParabolicMovementConfiguration")]
public class ProjectileParabolicMovementSO : ProjectileMovementSO
{
    protected override IProjectileMovement InitializeMovement()
    {
        return new ProjectileParabolicMovement();
    }
}
