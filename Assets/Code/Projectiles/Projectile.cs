using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Projectile : RecyclableObject
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
    private List<string> _targetLayerMasks;
    private Vector3 _targetLastPosition = Vector3.zero;
    private bool _isAboutToDestroy;

    private void Awake()
    {
        _transform = this.transform;
        _targetLayerMasks = new List<string>();
    }

    public void Initialize(ProjectileConfigurationSO configuration, float speed, Collider targetCollider, int damage, string[] targetLayerMasks, Vector3 spawnDirection)
    {
        _configuration = configuration;
        _movement = _configuration.Movement;
        _damager = _configuration.Damager;
        _currentLifetime = _configuration.MaximumLifetime;
        _speed = speed;
        _targetCollider = targetCollider;
        _damage = damage;
        _transform.rotation = Quaternion.LookRotation(spawnDirection, Vector3.up);
        InitializeTargetLayerMaskList(targetLayerMasks);

        _isInitialized = true;
    }

    private void InitializeTargetLayerMaskList(string[] targetLayerMasks)
    {
        if(_targetLayerMasks.Count > 0)
        {
            _targetLayerMasks.Clear();
        }

        foreach (string layer in targetLayerMasks)
        {
            _targetLayerMasks.Add(layer);
        }
    }

    private void Update()
    {
        if(!_isInitialized)
        {
            return;
        }

        UpdateMovement();
        UpdateLifetimeCounter();

        if(_isAboutToDestroy)
        {
            DestroyProjectile();
        }
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
            _isAboutToDestroy = true;
            return;
        }

        ApplyMovement(newPosition);
    }

    private void UpdateLifetimeCounter()
    {
        _currentLifetime -= Time.deltaTime;

        if(_currentLifetime <= 0)
        {
            _isAboutToDestroy = true;
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
            _isAboutToDestroy = true;
        }
    }

    private void DestroyProjectile()
    {
        Recycle();
    }

    public override void Init()
    {

    }

    public override void Release()
    {
        Reset();
    }

    private void Reset()
    {
        _isInitialized = false;
        _configuration = null;
        _targetCollider = null;
        _currentLifetime = 0f;
        _damager = null;
        _movement = null;
        _damage = 0;
        _speed = 0f;
        _targetLastPosition = Vector3.zero;
        _targetLayerMasks.Clear();
        _isAboutToDestroy = false;
    }
}
