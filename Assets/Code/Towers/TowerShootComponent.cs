using System;
using UnityEngine;

public class TowerShootComponent
{
    private readonly ProjectileSpawner _projectileSpawner;
    private readonly ProjectileConfigurationSO _projectileConfiguration;
    private readonly float _fireRate;
    private bool _canShoot = true;
    private float _lastShotTime = 0f;

    public event Action OnShotPerformed;

    public TowerShootComponent(ProjectileSpawner projectileSpawner, float fireRate, ProjectileConfigurationSO projectileConfiguration)
    {
        _projectileSpawner = projectileSpawner;
        _fireRate = fireRate;
        _projectileConfiguration = projectileConfiguration;
    }

    public void Update()
    {
        UpdateShootingAvailability();
    }

    private void UpdateShootingAvailability()
    {
        if(!_canShoot)
        {
            if (Time.time - _lastShotTime >= _fireRate)
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

        _projectileSpawner.Spawn(shotPosition, shotDirection, 10f, _projectileConfiguration, targetTransform);
        OnShotPerformed?.Invoke();
    }

    public bool CanShoot => _canShoot;
}