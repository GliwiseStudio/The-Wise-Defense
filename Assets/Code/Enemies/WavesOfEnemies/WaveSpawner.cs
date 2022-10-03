using System.Collections;
using System.Collections.Generic;
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
        //_timeBetweenWaves = _currentWave.TimeBeforeThisWave;
        _timeBetweenWaves = 1f; // temporarily for testing
    }

    private void Update()
    {
        if (_allWavesSpawned) // all waves have been spawned, no need to progress further down the code
        {
            return;
        }

        if (Time.time >= _timeBetweenWaves)
        {
            StartCoroutine(SpawnWave());
            IncWave();

            _timeBetweenWaves = Time.time + _currentWave.TimeBeforeThisWave;
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
        int numberOfEnemies = _currentWave.EnemiesInWave.Length;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(_currentWave.EnemiesInWave[i], spawnPoint.position, spawnPoint.rotation);

            yield return new WaitForSeconds(0.5f); // time to wait between enemies spawning
        }
    }
}
