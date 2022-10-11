using System;
using UnityEngine;

[Serializable]
public class TowerUpgrade
{
    [SerializeField] private int _damage = -1;
    [SerializeField] private float _range = -1f;
    [SerializeField] private float _fireRate = -1f;
    [SerializeField] private GameObject _gameObject;

    public int Damage => _damage;
    public float Range => _range;
    public float FireRate => _fireRate;
    public GameObject GameObject => _gameObject;

    public TowerUpgrade() { }
}
