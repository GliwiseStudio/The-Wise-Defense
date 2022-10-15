using UnityEngine;
using UnityEngine.UI;

public class DamageableObstacle: MonoBehaviour, IDamage, IHeal
{
    [SerializeField] private float _maxHealth = 500;
    private Slider _slider;
    private Camera _sceneCamera;

    private float _currentHealth;

    void Start()
    {
        _slider = GetComponentInChildren<Slider>();
        _sceneCamera = FindObjectOfType<Camera>();

        _currentHealth = _maxHealth;
        _slider.value = 1; 
        _slider.transform.LookAt(_sceneCamera.transform.position);
    }

    #region Interface methods
    public void ReceiveDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }

        _slider.value = _currentHealth / _maxHealth;
    }

    public void Heal(int healAmount)
    {
        _currentHealth += healAmount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        _slider.value = _currentHealth / _maxHealth;
    }
    #endregion
}
