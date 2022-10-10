using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Projectile/ProjectileConfiguration", fileName = "ProjectileConfiguration")]
public class ProjectileConfigurationSO : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _maximumLifetime = 5f;
    [SerializeField] private ProjectileMovementSO _movementType;
    [SerializeField] private ProjectileDamagerSO _damageType;

    public GameObject Prefab => _prefab;
    public float MaximumLifetime => _maximumLifetime;
    public IProjectileMovement Movement => _movementType.Movement;
    public IProjectileDamager Damager => _damageType.Damager;
}
