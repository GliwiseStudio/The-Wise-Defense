using UnityEngine;

public class EnemyMovement
{
    //[SerializeField] private float _speed = 5f;

    private readonly Transform _enemyTransform;

    private float _speed;
    private Vector3 _dir;
    private Transform _target; // next point on the way
    private int _targetIdx = 0; // index of the target waypoint on the waypoints array
    private bool _reachedEnd = false;

    public EnemyMovement(Transform enemyTransform, float speed)
    {
        _enemyTransform = enemyTransform;
        _speed = speed;

        _target = Waypoints.waypoints[_targetIdx]; // initialice target

        CalculateDirection();
    }

    public void Update()
    {
        if(_reachedEnd == false)
            TranslateEnemy();
    }

    void TranslateEnemy()
    {
        _enemyTransform.Translate(_dir * _speed * Time.deltaTime, Space.World); // translate the enemy across that direction, ensuring that the movement speed is only dependant of the speed attribute

        if (Vector3.Distance(_enemyTransform.position, _target.position) <= 0.2f) // if the enemy has reached the waypoint
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        _targetIdx++;

        if (_targetIdx < Waypoints.waypoints.Length)
        {
            _target = Waypoints.waypoints[_targetIdx];

            CalculateDirection();
        }
        else // the object has reached it's destination
        {
            _reachedEnd = true;
        }
    }

    void CalculateDirection()
    {
        _dir = (_target.position - _enemyTransform.position).normalized; // calculate direction of movement
        _enemyTransform.LookAt(_target); // look in the direction of the target
    }

    public void UpdateSpeed(float speed)
    {
        _speed = speed;
    }

}
