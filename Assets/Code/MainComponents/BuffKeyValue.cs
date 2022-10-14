using System;
using UnityEngine;

[Serializable]
public class BuffKeyValue
{
    [SerializeField] private string _key;
    [SerializeField] private float _value;
    [SerializeField] private float _duration;
    private float _durationLeft;
    private bool _isActive;

    public string Key => _key;
    public float Value => _value;
    public float Duration => _duration;
    public void DecreaseDuration(float decreaseValue)
    {
        _durationLeft -= decreaseValue;

        if(_durationLeft <= 0f)
        {
            _durationLeft = 0f;
            _isActive = false;
        }
    }
    public bool IsActive => _isActive;

    public BuffKeyValue(string key, float value, float duration)
    {
        _key = key;
        _value = value;
        _duration = duration;
        _durationLeft = duration;
        _isActive = true;
    }
}
