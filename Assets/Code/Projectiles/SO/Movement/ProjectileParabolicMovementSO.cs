using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Projectile/Movement Type/ParabolicMovement", fileName = "ProjectileParabolicMovementConfiguration")]
public class ProjectileParabolicMovementSO : ProjectileMovementSO
{
    [SerializeField] private AnimationCurve _trajectoryCurve;

    protected override IProjectileMovement InitializeMovement()
    {
        return new ProjectileParabolicMovement(_trajectoryCurve);
    }
}
