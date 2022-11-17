using System;
using UnityEngine;

[Serializable]
public class TowerUpgrade
{
    [SerializeField] [Range(1, 3)] private int _level = 1;
    [SerializeField] private int _damage = -1;
    [SerializeField] private float _range = -1f;
    [SerializeField] private float _fireRate = -1f;

    public int Level => _level;
    public int Damage => _damage;
    public float Range => _range;
    public float FireRate => _fireRate;

    public TowerUpgrade() { }
}
