using System;
using UnityEngine;

public class TowerShootComponent
{
    private readonly ProjectileSpawner _projectileSpawner;
    private readonly ProjectileConfigurationSO _projectileConfiguration;
    private readonly TowerShootingConfiguration _shootingConfiguration;
    private bool _canShoot = true;
    private float _lastShotTime = 0f;
    private string[] _targetLayerMasks;

    private bool _isDamageBuffed;
    private int _buffedDamage;

    public event Action OnShotPerformed;

    public TowerShootComponent(ProjectileSpawner projectileSpawner, TowerShootingConfiguration shootingConfiguration, ProjectileConfigurationSO projectileConfiguration, string[] targetLayerMasks)
    {
        _projectileSpawner = projectileSpawner;
        _shootingConfiguration = shootingConfiguration;
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
            if (Time.time - _lastShotTime >= _shootingConfiguration.FireRate)
            {
                _canShoot = true;
            }
        }
    }

    public void Shoot(Vector3 shotPosition, Vector3 shotDirection, Transform targetTransform)
    {
        if(!_canShoot)
        {
            return;
        }

        _canShoot = false;
        _lastShotTime = Time.time;

        int damage = GetDamage();

        _projectileSpawner.Spawn(shotPosition, shotDirection, 10f, _projectileConfiguration, targetTransform, damage, _targetLayerMasks);
        OnShotPerformed?.Invoke();
    }

    private int GetDamage()
    {
        int returnedDamage = _shootingConfiguration.Damage;

        if(_isDamageBuffed)
        {
            returnedDamage += _buffedDamage;
        }

        return returnedDamage;
    }

    public void BuffDamage(int buffedDamage)
    {
        _isDamageBuffed = true;
        _buffedDamage = buffedDamage;
    }

    public void UnbuffDamage()
    {
        _isDamageBuffed = false;
        _buffedDamage = 0;
    }

    public bool CanShoot => _canShoot;
}