using UnityEngine;

public class EnemyMovement
{
    //[SerializeField] private float _speed = 5f;

    private readonly Transform _enemyTransform;

    private float _speed;
    private Vector3 _dir;
    private Transform _targetWp; // next point on the way
    private Transform _obstacleTransform;
    private int _targetWpIdx = 0; // index of the target waypoint on the waypoints array
    private bool _reachedEnd = false;
    private bool _obstacleDetected = false;

    private float _randomDistance = Random.Range(0.5f, 2f);

    public bool ObstacleReached = false;

    public EnemyMovement(Transform enemyTransform, float speed)
    {
        _enemyTransform = enemyTransform;
        _speed = speed;

        _targetWp = Waypoints.waypoints[_targetWpIdx]; // initialice target

        CalculateWaypointDirection();
    }

    public void Update()
    {
        if (_obstacleDetected)
        {
            if (!ObstacleReached)
            {
                TranslateEnemyToObstacle();
            }
        }
        else
        {
            if (!_reachedEnd)
            {
                TranslateEnemyThroughWaypoints();
            }
        }
    }

    #region Standard movement
    void TranslateEnemyThroughWaypoints()
    {
        _enemyTransform.Translate(_dir * _speed * Time.deltaTime, Space.World); // translate the enemy across that direction, ensuring that the movement speed is only dependant of the speed attribute

        if (Vector3.Distance(_enemyTransform.position, _targetWp.position) <= 0.2f) // if the enemy has reached the waypoint
        {
            GetNextWaypoint();
        }
    }
    void GetNextWaypoint()
    {
        _targetWpIdx++;

        if (_targetWpIdx < Waypoints.waypoints.Length)
        {
            _targetWp = Waypoints.waypoints[_targetWpIdx];

            CalculateWaypointDirection();
        }
        else // the object has reached it's destination
        {
            _reachedEnd = true;
        }
    }

    public void CalculateWaypointDirection()
    {
        ObstacleReached = false;
        _obstacleDetected = false;

        _dir = (_targetWp.position - _enemyTransform.position).normalized; // calculate direction of movement

        _enemyTransform.LookAt(_targetWp); // look in the direction of the target
    }

    #endregion

    #region Obstacle in range related movement
    void TranslateEnemyToObstacle()
    {
        _enemyTransform.Translate(_dir * _speed * Time.deltaTime, Space.World); // translate the enemy across that direction, ensuring that the movement speed is only dependant of the speed attribute

        Ray ray = new Ray(_enemyTransform.position, _dir);
        if (Physics.Raycast(ray, _randomDistance))
        {
            ObstacleReached = true;
            // The Ray hit something!
        }

        //if (Vector3.Distance(_enemyTransform.position, _obstacleTransform.position) <= _randomDistance) // if the enemy has reached the waypoint
        //{
        //    ObstacleReached = true;
        //}
    }
    public void CalculateObstacleDirection(Transform obstacleTransform)
    {
        _obstacleDetected = true;

        _obstacleTransform = obstacleTransform;

        _dir = (_obstacleTransform.position - _enemyTransform.position).normalized; // calculate direction of movement

        _enemyTransform.LookAt(_obstacleTransform); // look in the direction of the target
    }

    #endregion

    public void UpdateSpeed(float speed)
    {
        _speed = speed;
    }

}
