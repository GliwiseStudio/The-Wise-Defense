using UnityEngine;

public class ProjectileStraightMovement : IProjectileMovement
{
    public Vector3 UpdateMovement(Vector3 currentPosition, Vector3 targetPosition, float speed)
    {
        return Vector3.Lerp(currentPosition, targetPosition, speed * Time.deltaTime);
    }
}