using System;
using UnityEngine;

[Serializable]
public class TowerUpgrade
{
    [SerializeField] private int _damage = -1;
    [SerializeField] private float _range = -1f;
    [SerializeField] private float _fireRate = -1f;
    [SerializeField] private GameObject _gameObject;

    public TowerUpgrade() { }
}
