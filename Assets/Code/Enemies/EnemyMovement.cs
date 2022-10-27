using UnityEngine;

public class EnemyMovement
{
    //[SerializeField] private float _speed = 5f;

    private readonly Transform _enemyTransform;

    private float _speed;
    private Vector3 _dir;
    private Transform _targetWp; // next point on the way
    private Transform _obstacleTransform;
    private GameObject _obstacleGameObject;
    private int _targetWpIdx = 0; // index of the target waypoint on the waypoints array
    private bool _reachedEnd = false;

    private float _randomDistance;

    private Waypoints _waypoints;

    public bool ObstacleReached = false;
    public bool ObstacleDetected = false;

    private TargetDetector _obstacleDetector;
    private string _obstaclesLayerMask;

    public EnemyMovement(Transform enemyTransform, float speed, string obstaclesLayerMask, Waypoints waypoints)
    {
        _enemyTransform = enemyTransform;
        _speed = speed;
        _obstaclesLayerMask = obstaclesLayerMask;
        _waypoints = waypoints;

        _randomDistance = Random.Range(0.2f, 1f);

        _targetWp = _waypoints.waypoints[_targetWpIdx]; // initialice target

        _obstacleDetector = new TargetDetector(_enemyTransform, _randomDistance, _obstaclesLayerMask);

        CalculateWaypointDirection();
    }

    public void Update()
    {
        if (ObstacleDetected)
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

        if (_targetWpIdx < _waypoints.waypoints.Length)
        {
            _targetWp = _waypoints.waypoints[_targetWpIdx];

            CalculateWaypointDirection();
        }
        else // the object has reached it's destination
        {
            _reachedEnd = true;
        }
    }

    void CalculateWaypointDirection() // not necessary to be public, since, because the obstacles will always be on the path, we don't need to recalculate direction after fight
    {
        _dir = (_targetWp.position - _enemyTransform.position).normalized; // calculate direction of movement

        _enemyTransform.LookAt(_targetWp); // look in the direction of the target
    }

    #endregion

    #region Obstacle in range related movement

    public void ResetObstacleDetectionState()
    {
        ObstacleReached = false;
        ObstacleDetected = false;
    }
    public void TranslateEnemyToObstacle()
    {
        _enemyTransform.Translate(_dir * _speed * Time.deltaTime, Space.World); // translate the enemy across that direction, ensuring that the movement speed is only dependant of the speed attribute

        if (_obstacleTransform != null && _obstacleGameObject.layer == LayerMask.NameToLayer(_obstaclesLayerMask)) // if there's an obstacle
        {
            ObstacleReached = true;
        }
        else // no obstacles
        {
            // detect if there's an obstacle in range
            _obstacleGameObject = _obstacleDetector.DetectTargetGameObject();
            _obstacleTransform = _obstacleDetector.DetectTarget();
        }
    }

    /* // Not necessary since the obstacles will always be on the path, in the same direction of the movement
    public void CalculateObstacleDirection(Transform obstacleTransform)
    {
        _obstacleDetected = true;

        _dir = (obstacleTransform.position - _enemyTransform.position).normalized; // calculate direction of movement

        _enemyTransform.LookAt(obstacleTransform); // look in the direction of the target
    }*/

    #endregion

    public void UpdateSpeed(float speed)
    {
        _speed = speed;
    }

}
