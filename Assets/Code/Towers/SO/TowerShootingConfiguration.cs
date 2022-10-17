using System;
using UnityEngine;

[Serializable]
public class TowerShootingConfiguration
{
    [SerializeField] private float _fireRate = 0.8f;
    [SerializeField] private int _damage = 10;

    public float FireRate => _fireRate;
    public int Damage => _damage;
    public void SetFirerate(float firerate)
    {
        _fireRate = firerate;
    }
    public void SetDamage(int damage)
    {
        _damage = damage;
    }
}
