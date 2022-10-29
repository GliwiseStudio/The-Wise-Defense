using UnityEngine;
using UnityEngine.UI;

public class DamageableTower : MonoBehaviour, IDamage, IHeal
{
    [SerializeField] private float _maxHealth = 500;
    private Slider _slider;
    private Camera _sceneCamera;

    private float _currentHealth;
    private float _timeOfDestruction;
    private bool _dissolveActivated = false;

    private Material[] _materials;

    void Start()
    {
        _slider = GetComponentInChildren<Slider>();
        _sceneCamera = FindObjectOfType<Camera>();
        _materials = GetComponent<Renderer>().materials;

        _currentHealth = _maxHealth;
        _slider.value = 1;
        _slider.transform.LookAt(_sceneCamera.transform.position);
    }

    private void Update()
    {
        if (_dissolveActivated)
        {
            Dissolve();
        }
    }

    #region Interface methods
    public void ReceiveDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
        {
            DestroyTower();
            return;
        }

        _slider.value = _currentHealth / _maxHealth;
    }

    #region Destruction methods
    private void DestroyTower()
    {
        Destroy(_slider.gameObject);

        int DeadEnemyLayer = LayerMask.NameToLayer("DeadEnemy");
        gameObject.layer = DeadEnemyLayer;

        _timeOfDestruction = Time.time;

        _dissolveActivated = true;
    }

    private void Dissolve()
    {
        if (Time.time - _timeOfDestruction < 1) // makes Progress property of shader go from 1 to 0 in the span of 1 second
        {
            float i = Time.time - _timeOfDestruction;
            foreach (Material _mat in _materials)
            {
                _mat.SetFloat("_DissolveProgress", i);
            }
        }
        else // when a second has passed, destroy the gameObject
        {
            GameManager.Instance.RemoveTower();
            Destroy(gameObject);
        }
    }

    #endregion

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