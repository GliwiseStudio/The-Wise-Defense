using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private EnemyWave[] _waves;
    [SerializeField] private Transform _spawnPoint;

    private EnemyWave _currentWave;
    private int _currentWaveNumber = -1;
    
    private void Awake()
    {
        _currentWave = _waves[0];
    }

    private void Update()
    {
        if (GameManager.Instance.GetIsWaveActive() && _currentWaveNumber != GameManager.Instance.GetCurrentWave()) // only happens once per wave
        {
            _currentWaveNumber = GameManager.Instance.GetCurrentWave();
            _currentWave = _waves[_currentWaveNumber];
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        EnemyWave currentWave = _currentWave;

        for (int i = 0; i < currentWave.EnemyTypesInWave.Length; i++)
        {
            for (int j = 0; j < currentWave.NumberOfEnemiesPerType[i]; j++)
            {
                Instantiate(currentWave.EnemyTypesInWave[i], _spawnPoint.position, _spawnPoint.rotation);
                yield return new WaitForSeconds(currentWave.TimeBetweenEnemies); // time to wait between enemies spawning
            }
        }
    }
}
