using UnityEngine;

public interface IProjectileMovement
{
    public Vector3 UpdateMovement(Transform projectileTransform, Transform targetTransform, float speed);
}
