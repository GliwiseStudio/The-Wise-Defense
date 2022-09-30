using UnityEngine;

public struct ProjectileMovementConfiguration
{
    public Vector3 currentPosition;
    public Vector3 currentNormalizedDirection;
    public float speed;

    public ProjectileMovementConfiguration(Vector3 currentPosition, Vector3 currentDirection, float speed)
    {
        this.currentPosition = currentPosition;
        this.currentNormalizedDirection = currentDirection;
        this.speed = speed;
    }
}
