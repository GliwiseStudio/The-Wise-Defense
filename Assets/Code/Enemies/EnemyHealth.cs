using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth
{
    private readonly float _maxHealth;
    private Slider _slider;
    private Camera _sceneCamera;

    private float _currentHealth;
    private bool _isEnemyDead = false;

    public EnemyHealth(float maxHealth, Slider slider)
    {
        _maxHealth = maxHealth;
        _slider = slider;
        //_sceneCamera = sceneCamera;

        _currentHealth = _maxHealth;
        _slider.value = 1;
    }

    public void Reset(Camera sceneCamera)
    {
        _sceneCamera = sceneCamera;
        _currentHealth = _maxHealth;
        _slider.value = 1;
        _isEnemyDead = false;
    }

    public void Update()
    {
        //ReceiveDamage(1); // temporarily for test purposes
        _slider.transform.LookAt(_sceneCamera.transform.position); // readjust healthBar lookAt
    }

    public void ReceiveDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
        {
            _slider.value = 0;
            _isEnemyDead = true;
            return;
        }

        _slider.value = _currentHealth / _maxHealth;
    }

    public string GetEnemyState()
    {
        if (_isEnemyDead == true)
            return "dead";
        else
            return "alive";
    }
}
