using UnityEngine;

public class ProjectileStraightMovement : IProjectileMovement
{
    private Vector3 _lastTargetPosition = Vector3.zero;

    public Vector3 UpdateMovement(Transform projectileTransform, Transform targetTransform, float speed)
    {
        if(targetTransform != null)
        {
            _lastTargetPosition = targetTransform.position;
        }

        return Vector3.Lerp(projectileTransform.position, _lastTargetPosition, speed * Time.deltaTime);
    }
}