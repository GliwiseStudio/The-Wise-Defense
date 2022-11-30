using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageableObstacle : MonoBehaviour, IDamage, IHeal
{
    private enum DamageableObstacleType { Tower, Barricade, Explosive_barricade }

    [Header("General Settings")]
    [SerializeField] private float _maxHealth = 500;
    [SerializeField] private DamageableObstacleType _osbtacleType;

    [Header("Explosive barricade settings")]
    [SerializeField] private int _dinamiteDamage;
    [SerializeField] private float _detectionRange = 2f;
    [SerializeField] private string[] _enemyLayers;

    private Slider _slider;
    private Camera _sceneCamera;
    
    private float _currentHealth;
    private float _timeOfDestruction;
    private bool _dissolveActivated = false;
    private bool _isInvulnerable = false;

    private Renderer[] _renderers;

    void Start()
    {
        _slider = GetComponentInChildren<Slider>();
        _sceneCamera = FindObjectOfType<Camera>();
        _renderers = GetComponentsInChildren<Renderer>();

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

    #region Destruction methods
    private void DestroyObstacle()
    {
        if (_osbtacleType == DamageableObstacleType.Explosive_barricade)
        {
            DinamiteExplosion();
        }

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
            foreach (Renderer renderer in _renderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    mat.SetFloat("_DissolveProgress", i);
                }
            }
        }
        else // when a second has passed, destroy the gameObject
        {
            if (_osbtacleType == DamageableObstacleType.Tower)
            {
                GameManager.Instance.RemoveTower();
            }
            Destroy(gameObject);
        }
    }

    private void DinamiteExplosion()
    {
        GetComponentInChildren<ParticleSystem>().Play(true);

        TargetDetector _enemyDetector = new TargetDetector(transform, _detectionRange, _enemyLayers);

        IReadOnlyList<Transform> enemies = _enemyDetector.GetAllTargetsInRange();
        foreach (Transform enemy in enemies)
        {
            enemy.gameObject.GetComponent<IDamage>().ReceiveDamage(_dinamiteDamage);
        }
    }

    #endregion

    #region Interface methods
    public void ReceiveDamage(int damageAmount)
    {
        if (!_isInvulnerable)
        {
            _currentHealth -= damageAmount;

            if (_currentHealth <= 0)
            {
                DestroyObstacle();
                return;
            }

            _slider.value = _currentHealth / _maxHealth;
        }
    }

    public void Heal(float healPercentage)
    {
        Debug.Log("here:" + _currentHealth);
        _currentHealth += _maxHealth * healPercentage;

        Debug.Log("after:" + _currentHealth);

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        _slider.value = _currentHealth / _maxHealth;
    }

    public bool FullHeal()
    {
        if(_currentHealth == _maxHealth)
        {
            return false;
        }
        else
        {
            _currentHealth = _maxHealth;
            _slider.value = _currentHealth / _maxHealth;
            return true;
        }
    }

    public void BecomeInvulnerable(float duration)
    {
        _isInvulnerable = true;
        ChangeObstacleColor(new Color32(0, 122, 254, 1)); // blue color
        StartCoroutine(ReleaseInvulnerabilty(duration));
    }

    IEnumerator ReleaseInvulnerabilty(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isInvulnerable = false;
        ChangeObstacleColor(new Color32(255, 255, 255, 1)); // change color back to normal
    }

    #endregion

    #region Color Change
    public void ChangeObstacleColor(Color32 color)
    {

        foreach (Renderer renderer in _renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                mat.SetColor("_MultiplyColor", color);
            }
        }
    }
    #endregion
}