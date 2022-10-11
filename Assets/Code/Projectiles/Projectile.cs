using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform _transform;
    private Transform _targetTransform;
    private float _speed = 3f;
    private int _damage = 10;
    private bool _isInitialized = false;
    private ProjectileConfigurationSO _configuration;
    private IProjectileMovement _movement;
    private IProjectileDamager _damager;
    private float _currentLifetime;
    private string[] _targetLayerMasks;

    private void Awake()
    {
        _transform = this.transform;
    }

    public void Initialize(ProjectileConfigurationSO configuration, float speed, Transform targetTransform, int damage, string[] targetLayerMasks)
    {
        _configuration = configuration;
        _movement = _configuration.Movement;
        _damager = _configuration.Damager;
        _currentLifetime = _configuration.MaximumLifetime;
        _speed = speed;
        _targetTransform = targetTransform;
        _damage = damage;
        _targetLayerMasks = targetLayerMasks;

        _isInitialized = true;
    }

    private void Update()
    {
        if(!_isInitialized)
        {
            return;
        }

        UpdateMovement();
        UpdateLifetimeCounter();
    }

    private void UpdateMovement()
    {
        Vector3 newPosition = _movement.UpdateMovement(_transform, _targetTransform, _speed);
        ApplyMovement(newPosition);
    }

    private void UpdateLifetimeCounter()
    {
        _currentLifetime -= Time.deltaTime;

        if(_currentLifetime <= 0)
        {
            DestroyProjectile();
        }
    }

    private void ApplyMovement(Vector3 newPosition)
    {
        _transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer);
        if(other.gameObject.layer == LayerMask.NameToLayer("GroundEnemies"))
        {
            IDamage damageableEnemy = other.gameObject.GetComponent<IDamage>();
            _damager.ApplyDamage(_damage, damageableEnemy, other.transform);

            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
}
