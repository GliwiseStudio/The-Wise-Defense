using UnityEngine;

public interface IProjectileMovement
{
    public Vector3 UpdateMovement(Transform projectileTransform, Collider targetCollider, float speed);
}
