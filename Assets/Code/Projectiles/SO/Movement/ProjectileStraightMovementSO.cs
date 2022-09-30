using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Projectile/Movement Type/StraightMovement", fileName = "ProjectileStraightMovementConfiguration")]
public class ProjectileStraightMovementSO : ProjectileMovementSO
{
    protected override IProjectileMovement InitializeMovement()
    {
        return new ProjectileStraightMovement();
    }
}
