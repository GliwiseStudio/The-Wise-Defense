using UnityEngine;

public interface IProjectileMovement
{
    public Vector3 UpdateMovement(ProjectileMovementConfiguration movementConfiguration);
}
