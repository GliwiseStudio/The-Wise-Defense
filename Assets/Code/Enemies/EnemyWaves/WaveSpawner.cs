using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public EnemyWave[] waves;
    private EnemyWave _currentWave;
    private int _currentWaveNumber = 0;

    private float _timeBetweenWaves;

    public Transform spawnPoint;

    private bool _allWavesSpawned = false;
    
    private void Awake()
    {
        _currentWave = waves[0];
        _timeBetweenWaves = _currentWave.TimeBeforeThisWave;
    }

    private void Update()
    {
        if (_allWavesSpawned) // all waves have been spawned, no need to progress further down the code
        {
            return;
        }

        if (Time.time >= _timeBetweenWaves) // temporarily for testing, later each wave will occur after the player has managed their cards
        {
            StartCoroutine(SpawnWave());
            IncWave();

            _timeBetweenWaves = Time.time + _currentWave.TimeBeforeThisWave; // i'm not sure this will be necessary later (?)
        }
    }

    void IncWave()
    {
        _currentWaveNumber++;

        if (_currentWaveNumber == waves.Length) 
        {
            _allWavesSpawned = true;
            return;
        }

        _currentWave = waves[_currentWaveNumber];
    }

    IEnumerator SpawnWave()
    {
        EnemyWave currentWave = _currentWave;

        for (int i = 0; i < currentWave.EnemyTypesInWave.Length; i++)
        {
            for (int j = 0; j < currentWave.NumberOfEnemiesPerType[i]; j++)
            {
                Instantiate(currentWave.EnemyTypesInWave[i], spawnPoint.position, spawnPoint.rotation);
                yield return new WaitForSeconds(currentWave.TimeBetweenEnemies); // time to wait between enemies spawning
            }
        }
    }
}
