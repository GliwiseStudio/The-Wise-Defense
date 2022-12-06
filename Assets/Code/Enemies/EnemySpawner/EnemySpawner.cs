using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPoolSO[] _enemyPoolsSO;

    [SerializeField] private Transform _enemiesContainer;
    private Dictionary<EnemyTypes.EnemyTypesEnum, ObjectPool> _enemyPools;

    private void Awake()
    {
        _enemyPools = new Dictionary<EnemyTypes.EnemyTypesEnum, ObjectPool>();
        foreach(EnemyPoolSO enemyPoolSO in _enemyPoolsSO)
        {
            InitializePool(enemyPoolSO);
        }
    }

    private void InitializePool(EnemyPoolSO enemyPoolSO)
    {
        Debug.Log("Initialized pool");
        ObjectPool enemyPool = new ObjectPool(enemyPoolSO.EnemyRecyclableObject, _enemiesContainer);
        enemyPool.Init(enemyPoolSO.MaxPoolObjects);

        _enemyPools.Add(enemyPoolSO.EnemyRecyclableObject.GetComponent<EnemyController>().GetEnemyType(), enemyPool);
    }

    public void Spawn(EnemyTypes.EnemyTypesEnum enemyType, Transform spawnPoint, Waypoints waypoints)
    {
        Debug.Log(_enemyPools.ContainsKey(enemyType));
        ObjectPool enemyPool = _enemyPools[enemyType];
        EnemyController enemy = enemyPool.Spawn<EnemyController>(spawnPoint.position);

        enemy.Initialize(waypoints, spawnPoint);
    }

}
