using UnityEngine;

public class ProjectileParabolicMovement : IProjectileMovement
{
    private readonly AnimationCurve _trajectoryAnimationCurve;
    private bool _hasMovementStarted = false;
    private float _duration = 1f;
    private float _height = 2f;
    private float _currentTime = 0f;
    Vector3 _startPosition;
    Vector3 _endPosition;

    public ProjectileParabolicMovement(AnimationCurve trajectoryAnimationCurve)
    {
        _trajectoryAnimationCurve = trajectoryAnimationCurve;
    }

    public Vector3 UpdateMovement(ProjectileMovementConfiguration movementConfiguration)
    {
        if(!_hasMovementStarted)
        {
            _hasMovementStarted = true;
            _duration = movementConfiguration.speed;
            _startPosition = movementConfiguration.currentPosition;
        }

        _currentTime += Time.deltaTime;

        if(_currentTime < _duration)
        {
            float normalizedTime = _currentTime / _duration;
            float currentHeight = _trajectoryAnimationCurve.Evaluate(normalizedTime);
        }

        return Vector3.zero;
    }
}
