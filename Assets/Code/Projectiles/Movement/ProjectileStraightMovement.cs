using UnityEngine;

public class ProjectileStraightMovement : IProjectileMovement
{
    private Vector3 _lastTargetPosition = Vector3.zero;

    public Vector3 UpdateMovement(Transform projectileTransform, Collider targetCollider, float speed)
    {
        if(targetCollider != null)
        {
            _lastTargetPosition = targetCollider.bounds.center;
        }

        return Vector3.Lerp(projectileTransform.position, _lastTargetPosition, speed * Time.deltaTime);
    }
}