using UnityEngine;

public class TargetDetector
{
    private readonly Transform _transform;
    private readonly float _detectionRadius;
    private readonly string _targetLayerMaskName;

    public TargetDetector(Transform towerTransform, float detectionRadius, string targetLayerMaskString)
    {
        _transform = towerTransform;
        _detectionRadius = detectionRadius;
        _targetLayerMaskName = targetLayerMaskString;
    }

    public Transform DetectTarget()
    {
        Collider enemyFound = FindNearest();
        if (enemyFound == null)
        {
            return null;
        }

        return enemyFound.transform;
    }

    public GameObject DetectTargetGameObject()
    {
        Collider enemyFound = FindNearest();
        if (enemyFound == null)
        {
            return null;
        }

        return enemyFound.gameObject;
    }

    private Collider FindNearest()
    {
        Collider[] enemiesFound = Physics.OverlapSphere(_transform.position, _detectionRadius, LayerMask.GetMask(_targetLayerMaskName));
        if (enemiesFound.Length == 0)
        {
            return null;
        }

        float distance = GetSquareDistanceToPlayer(enemiesFound[0].transform.position);
        int closestIndex = 0;
        float currentEnemyDistance;

        for (int i = 1; i < enemiesFound.Length; i++)
        {
            currentEnemyDistance = GetSquareDistanceToPlayer(enemiesFound[i].transform.position);
            if (distance > currentEnemyDistance)
            {
                distance = currentEnemyDistance;
                closestIndex = i;
            }
        }

        return enemiesFound[closestIndex];
    }

    private float GetSquareDistanceToPlayer(Vector3 enemyPosition)
    {
        return (enemyPosition - _transform.position).sqrMagnitude;
    }

    public bool IsTargetInRange(Vector3 targetPosition)
    {
        if (GetSquareDistanceToPlayer(targetPosition) > (_detectionRadius * _detectionRadius))
        {
            return false;
        }

        return true;
    }
}
