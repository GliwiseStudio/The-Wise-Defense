using System;
using UnityEngine;

public class TowerShootComponent
{
    private readonly ProjectileSpawner _projectileSpawner;
    private readonly string _projectileType;
    private readonly float _fireRate;
    private bool _canShoot = true;
    private float _lastShotTime = 0f;

    public event Action OnShotPerformed;

    public TowerShootComponent(ProjectileSpawner projectileSpawner, string projectileType, float fireRate)
    {
        _projectileSpawner = projectileSpawner;
        _projectileType = projectileType;
        _fireRate = fireRate;
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

    public void Shoot(Vector3 shotPosition, Vector3 shotDirection)
    {
        if(!_canShoot)
        {
            return;
        }

        _canShoot = false;
        _lastShotTime = Time.time;
        Debug.Log(shotDirection);
        _projectileSpawner.Spawn(shotPosition, shotDirection, _projectileType);
        OnShotPerformed?.Invoke();
    }

    public bool CanShoot => _canShoot;
}