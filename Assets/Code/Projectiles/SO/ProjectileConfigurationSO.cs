using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Projectile/ProjectileConfiguration", fileName = "ProjectileConfiguration")]
public class ProjectileConfigurationSO : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private ProjectileMovementSO _movementType;

    public GameObject Prefab => _prefab;
    public IProjectileMovement Movement => _movementType.Movement();
}
