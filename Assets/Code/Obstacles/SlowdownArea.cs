using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class SlowdownArea : MonoBehaviour, IRemove
{
    [SerializeField][Range(0.1f, 0.9f)] private float _slowdownPercentage;
    
    [SerializeField] private float _maxEnemiesAllowed = 20;
    [SerializeField] private string[] _targerLayerMasks;
    [SerializeField] private bool _damageEnemies;

    [Header("If obstacle inflicts damage: ")]
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _time = 0.5f;

    private List<GameObject> _enemiesList;
    private float _enemiesAllowed;
    private Slider _slider;
    private Camera _sceneCamera;

    private void Start()
    {
        _enemiesList = new List<GameObject>();
        _slider = GetComponentInChildren<Slider>();
        _sceneCamera = FindObjectOfType<Camera>();

        _enemiesAllowed = _maxEnemiesAllowed;
        _slider.value = 1;
        _slider.transform.LookAt(_sceneCamera.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_targerLayerMasks.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            IDownStats enemy = other.gameObject.GetComponent<IDownStats>();
            enemy.ReceiveSlowdown(_slowdownPercentage);
            enemy.SetSlowdownObject(gameObject);
 
            if (_damageEnemies)
            {
                enemy.ReceiveDamageOnLoop(_damage, _time);
            }

            _enemiesList.Add(other.gameObject);
            UpdateSlider();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_targerLayerMasks.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            IDownStats enemy = other.gameObject.GetComponent<IDownStats>();
            enemy.ReleaseSlowdown(_slowdownPercentage);
            enemy.RemoveSlowdownObject();

            if (_damageEnemies)
            {
                enemy.StopDamageLoop();
            }

            _enemiesList.Remove(other.gameObject);
        }
    }

    public void RemoveFromList(GameObject enemy)
    {
        _enemiesList.Remove(enemy);
    }

    private void DestroyObstacle()
    {
        foreach (GameObject enemy in _enemiesList)
        {
            enemy.gameObject.GetComponent<IDownStats>().ReleaseSlowdown(_slowdownPercentage);
        }

        Destroy(gameObject);
    }

    private void UpdateSlider()
    {
        _enemiesAllowed--;

        if (_enemiesAllowed <= 0)
        {
            DestroyObstacle();
            return;
        }

        Debug.Log(_enemiesAllowed);
        Debug.Log(_maxEnemiesAllowed);

        _slider.value = _enemiesAllowed / _maxEnemiesAllowed;
        Debug.Log(_slider.value);
    }
}
