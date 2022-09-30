using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform _transform;
    private Transform _targetTransform;
    private float _speed = 3f;
    private IProjectileMovement _movement;
    private bool _isInitialized = false;
    private IProjectileDamager _damager;

    private void Awake()
    {
        _transform = this.transform;
    }

    public void Initialize(IProjectileMovement movement, IProjectileDamager damager, float speed, Transform targetTransform)
    {
        _movement = movement;
        _damager = damager;
        _speed = speed;
        _targetTransform = targetTransform;

        _isInitialized = true;
    }

    private void Update()
    {
        if(!_isInitialized)
        {
            return;
        }

        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector3 newPosition = _movement.UpdateMovement(_transform, _targetTransform, _speed);
        ApplyMovement(newPosition);
    }

    private void ApplyMovement(Vector3 newPosition)
    {
        _transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            IDamage damageableEnemy = other.gameObject.GetComponent<IDamage>();
            _damager.ApplyDamage(50, damageableEnemy);
        }
    }
}
