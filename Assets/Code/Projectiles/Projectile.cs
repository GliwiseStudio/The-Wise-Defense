using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform _transform;
    private float _speed = 15f;
    private IProjectileMovement _movement;
    //private IProjectileDamager _damager;

    private void Awake()
    {
        _transform = this.transform;
        _movement = new ProjectileStraightMovement();
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        ProjectileMovementConfiguration movementConfiguration = new ProjectileMovementConfiguration(_transform.position, _transform.forward, _speed);
        Vector3 newPosition = _movement.UpdateMovement(movementConfiguration);
        ApplyMovement(newPosition);
    }

    private void ApplyMovement(Vector3 newPosition)
    {
        _transform.position = newPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyInterface damageableEnemy = collision.gameObject.GetComponent<EnemyInterface>();
        //_damager.ApplyDamage(5);
    }
}
