using System;
using UnityEngine;

[Serializable]
public class BuffKeyValue
{
    [SerializeField] private string _key;
    [SerializeField][Range(-100, 100)] private int _buffPercentage;
    [SerializeField] private float _duration = 3f;
    private float _durationLeft = -100f;
    private bool _isActive = true;

    public string Key => _key;
    public int BuffPercentage => _buffPercentage;
    public float Duration => _duration;
    public void DecreaseDuration(float decreaseValue)
    {
        Debug.Log(_durationLeft);
        _durationLeft -= decreaseValue;

        if(_durationLeft <= 0f)
        {
            _durationLeft = 0f;
            _isActive = false;
        }
    }
    public bool IsActive => _isActive;

    public BuffKeyValue()
    {
        _durationLeft = _duration;
        _isActive = true;
        Debug.Log(_durationLeft);
    }

    public BuffKeyValue(string key, int buffPercentage, float duration)
    {
        _key = key;
        _buffPercentage = buffPercentage;
        _duration = duration;
        _durationLeft = duration;
        _isActive = true;
    }
}
