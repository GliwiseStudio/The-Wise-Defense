using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class SlowdownArea : MonoBehaviour, IRemove, IHeal
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
    private bool _isInvulnerable = false;

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

            if (!_isInvulnerable)
            {
                UpdateSlider();
            }
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

        _slider.value = _enemiesAllowed / _maxEnemiesAllowed;
    }

    #region Interfaces
    public void RemoveFromList(GameObject enemy)
    {
        _enemiesList.Remove(enemy);
    }

    public void Heal(int healAmount)
    {
        _enemiesAllowed += healAmount;

        if (_enemiesAllowed > _maxEnemiesAllowed)
        {
            _enemiesAllowed = _maxEnemiesAllowed;
        }

        _slider.value = _enemiesAllowed / _maxEnemiesAllowed;
    }

    public bool FullHeal()
    {
        if (_enemiesAllowed == _maxEnemiesAllowed)
        {
            return false;
        }
        else
        {
            _enemiesAllowed = _maxEnemiesAllowed;
            _slider.value = _enemiesAllowed / _maxEnemiesAllowed;
            return true;
        }
    }

    public void BecomeInvulnerable(float duration)
    {
        _isInvulnerable = true;
        //ChangeObstacleColor(new Color32(0, 122, 254, 1)); // blue color
        StartCoroutine(ReleaseInvulnerabilty(duration));
    }

    IEnumerator ReleaseInvulnerabilty(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isInvulnerable = false;
        //ChangeObstacleColor(new Color32(255, 255, 255, 1)); // change color back to normal
    }
    #endregion

    // i'd have to change the material to the ones with the dissolve shader
    //#region Color Change
    //public void ChangeObstacleColor(Color32 color)
    //{

    //    foreach (Renderer renderer in _renderers)
    //    {
    //        foreach (Material mat in renderer.materials)
    //        {
    //            mat.SetColor("_MultiplyColor", color);
    //        }
    //    }
    //}
    //#endregion

}


