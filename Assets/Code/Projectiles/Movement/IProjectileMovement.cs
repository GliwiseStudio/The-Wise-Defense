using UnityEngine;

public interface IProjectileMovement
{
    public Vector3 UpdateMovement(Vector3 currentPosition, Vector3 targetPosition, float speed);
}
