using UnityEngine;

public class ProjectileStraightMovement : IProjectileMovement
{
    public Vector3 UpdateMovement(ProjectileMovementConfiguration movementConfiguration)
    {
        return movementConfiguration.currentPosition + (movementConfiguration.currentNormalizedDirection * movementConfiguration.speed * Time.deltaTime);
    }
}