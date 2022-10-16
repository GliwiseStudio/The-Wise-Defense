public class BuffKeyValue
{
    private string _key;
    private int _buffPercentage;
    private float _duration;
    private float _durationLeft;
    private bool _isActive;

    public string Key => _key;
    public int BuffPercentage => _buffPercentage;
    public float Duration => _duration;
    public void DecreaseDuration(float decreaseValue)
    {
        _durationLeft -= decreaseValue;

        if (_durationLeft <= 0f)
        {
            _durationLeft = 0f;
            _isActive = false;
        }
    }

    public bool IsActive => _isActive;

    public BuffKeyValue(string key, int buffPercentage, float duration)
    {
        _key = key;
        _buffPercentage = buffPercentage;
        _duration = duration;
        _durationLeft = duration;
        _isActive = true;
    }
}
