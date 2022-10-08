using UnityEngine;

[DisallowMultipleComponent]
public class ProjectileSpawner : MonoBehaviour
{
    public void Spawn(Vector3 spawnPosition, Vector3 spawnDirection, float speed, ProjectileConfigurationSO projectileConfiguration, Transform targetTransform, int damage)
    {
        Debug.Log("Spawn A Projectile! ->->->->->->");
        GameObject spawnedProjectile = Instantiate(projectileConfiguration.Prefab, spawnPosition, Quaternion.LookRotation(spawnDirection, Vector3.up));
        Projectile projectile = spawnedProjectile.GetComponent<Projectile>();

        projectile.Initialize(projectileConfiguration.Movement, projectileConfiguration.Damager, speed, targetTransform, damage);
    }
}
