using UnityEngine;

[DisallowMultipleComponent]
public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private RecyclableObject _projectilePrefab;
    [SerializeField] private Transform _projectilesContainer;
    [SerializeField] private int _objectPoolMaximumCapacity = 15;
    private ObjectPool _projectilesPool;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        _projectilesPool = new ObjectPool(_projectilePrefab, _projectilesContainer);
        _projectilesPool.Init(_objectPoolMaximumCapacity);
    }

    public void Spawn(Vector3 spawnPosition, Vector3 spawnDirection, float speed, ProjectileConfigurationSO projectileConfiguration, Collider targetCollider, int damage, string[] targetLayerMasks)
    {
        Projectile projectile = _projectilesPool.Spawn<Projectile>(spawnPosition);

        projectile.Initialize(projectileConfiguration, speed, targetCollider, damage, targetLayerMasks, spawnDirection);
    }
}
