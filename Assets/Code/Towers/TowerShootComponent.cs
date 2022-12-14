using System;
using UnityEngine;

public class TowerShootComponent
{
    private readonly ProjectileSpawner _projectileSpawner;
    private readonly ProjectileConfigurationSO _projectileConfiguration;
    private int _damage;
    private float _firerate;
    private bool _canShoot = true;
    private float _lastShotTime = 0f;
    private string[] _targetLayerMasks;

    private bool _isDamageBuffed = false;
    private int _buffedDamagePercentage;

    private bool _isFireRateBuffed = false;
    private int _buffedFireRatePercentage;

    public event Action OnShotPerformed;

    public TowerShootComponent(ProjectileSpawner projectileSpawner, int damage, float firerate, ProjectileConfigurationSO projectileConfiguration, string[] targetLayerMasks)
    {
        _projectileSpawner = projectileSpawner;
        _damage = damage;
        _firerate = firerate;
        _projectileConfiguration = projectileConfiguration;
        _targetLayerMasks = targetLayerMasks;
    }

    public void Update()
    {
        UpdateShootingAvailability();
    }

    private void UpdateShootingAvailability()
    {
        if(!_canShoot)
        {
            float fireRate = GetFireRate();
            if (Time.time - _lastShotTime >= fireRate)
            {
                _canShoot = true;
            }
        }
    }

    public void Shoot(Vector3 shotPosition, Vector3 shotDirection, Collider targetCollider)
    {
        if(!_canShoot)
        {
            return;
        }

        _canShoot = false;
        _lastShotTime = Time.time;

        int damage = GetDamage();

        _projectileSpawner.Spawn(shotPosition, shotDirection, 10f, _projectileConfiguration, targetCollider, damage, _targetLayerMasks);
        OnShotPerformed?.Invoke();
    }

    private int GetDamage()
    {
        return _damage + ((_damage * _buffedDamagePercentage) / 100);
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void SetFirerate(float firerate)
    {
        _firerate = firerate;
    }

    private float GetFireRate()
    {
        return _firerate + ((_firerate * _buffedFireRatePercentage) / 100);
    }

    public void BuffDamage(int buffedDamage)
    {
        _isDamageBuffed = true;
        _buffedDamagePercentage = buffedDamage;
    }

    public void UnbuffDamage()
    {
        _isDamageBuffed = false;
        _buffedDamagePercentage = 0;
    }

    public void BuffFireRate(int buffedFireRate)
    {
        _isFireRateBuffed = true;
        _buffedFireRatePercentage = buffedFireRate;
    }

    public void UnbuffFireRate()
    {
        _isFireRateBuffed = false;
        _buffedFireRatePercentage = 0;
    }

    public bool CanShoot => _canShoot;
}