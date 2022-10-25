using UnityEngine;

public class ProjectileParabolicMovement : IProjectileMovement
{
    private readonly AnimationCurve _trajectoryAnimationCurve;
    private bool _hasMovementStarted = false;
    private float _duration = 1f;
    private float _currentTime = 0f;
    private Vector3 _endPosition = Vector3.zero;

    public ProjectileParabolicMovement(AnimationCurve trajectoryAnimationCurve)
    {
        _trajectoryAnimationCurve = trajectoryAnimationCurve;
    }

    public Vector3 UpdateMovement(Transform projectileTransform, Collider targetCollider, float speed)
    {
        if(!_hasMovementStarted)
        {
            _hasMovementStarted = true;
            _duration = speed;
        }

        
        if(targetCollider != null)
        {
            _endPosition = targetCollider.bounds.center;
        }

        _currentTime += Time.deltaTime;

        if(_currentTime < _duration)
        {
            float normalizedTime = _currentTime / _duration;
            float currentHeight = _trajectoryAnimationCurve.Evaluate(normalizedTime);
            return Vector3.MoveTowards(projectileTransform.position, _endPosition, normalizedTime) + (Vector3.up * 0.05f);
        }

        return Vector3.zero;
    }
}
