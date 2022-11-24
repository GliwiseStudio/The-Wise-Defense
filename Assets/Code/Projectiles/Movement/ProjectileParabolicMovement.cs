using UnityEngine;

public class ProjectileParabolicMovement : IProjectileMovement
{
    private bool _hasMovementStarted = false;
    private float _duration = 1f;
    private float _currentTime = 0f;

    public ProjectileParabolicMovement() { }

    public Vector3 UpdateMovement(Vector3 currentPosition, Vector3 targetPosition, float speed)
    {
        if(!_hasMovementStarted)
        {
            _hasMovementStarted = true;
            _duration = speed;
        }

        _currentTime += Time.deltaTime;

        if(_currentTime < _duration)
        {
            float normalizedTime = _currentTime / _duration;
            return Vector3.MoveTowards(currentPosition, targetPosition, normalizedTime) + (Vector3.up * 0.05f);
        }

        return Vector3.zero;
    }
}
