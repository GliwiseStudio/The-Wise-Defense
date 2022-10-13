using System;
using UnityEngine;

[Serializable]
public class BuffKeyValue
{
    [SerializeField] private string _key;
    [SerializeField] private float _value;

    public string Key => _key;
    public float Value => _value;

    public BuffKeyValue(string key, float value)
    {
        _key = key;
        _value = value;
    }
}
