using UnityEngine;

[DisallowMultipleComponent]
public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;

    public void Spawn(Vector3 spawnPosition, Vector3 spawnDirection, string projectileType)
    {
        Debug.Log("Spawn A Projectile! ->->->->->->");
        Instantiate(_projectilePrefab, spawnPosition, Quaternion.LookRotation(spawnDirection, Vector3.up));
    }
}
