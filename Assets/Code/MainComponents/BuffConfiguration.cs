using System;
using UnityEngine;

[Serializable]
public class BuffConfiguration
{
    [SerializeField] private string _key;
    [SerializeField] [Range(-100, 100)] private int _buffPercentage = 25;
    [SerializeField] private float _duration = 3f;

    public string Key => _key;
    public int BuffPercentage => _buffPercentage;
    public float Duration => _duration;
}
