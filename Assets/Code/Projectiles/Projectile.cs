using UnityEngine;
using System.Linq;

public class Projectile : MonoBehaviour
{
    private Transform _transform;
    private Collider _targetCollider;
    private float _speed = 3f;
    private int _damage = 10;
    private bool _isInitialized = false;
    private ProjectileConfigurationSO _configuration;
    private IProjectileMovement _movement;
    private IProjectileDamager _damager;
    private float _currentLifetime;
    private string[] _targetLayerMasks;
    private Vector3 _targetLastPosition = Vector3.zero;

    private void Awake()
    {
        _transform = this.transform;
    }

    public void Initialize(ProjectileConfigurationSO configuration, float speed, Collider targetCollider, int damage, string[] targetLayerMasks)
    {
        _configuration = configuration;
        _movement = _configuration.Movement;
        _damager = _configuration.Damager;
        _currentLifetime = _configuration.MaximumLifetime;
        _speed = speed;
        _targetCollider = targetCollider;
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
        if(_targetCollider != null)
        {
            _targetLastPosition = _targetCollider.bounds.center;
        }

        Vector3 newPosition = _movement.UpdateMovement(_transform.position, _targetLastPosition, _speed);

        if(newPosition == _transform.position)
        {
            DestroyProjectile();
            return;
        }

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
        if(_targetLayerMasks.Contains(LayerMask.LayerToName(other.gameObject.layer)))
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
