using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private EnemyWave[] _waves;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Waypoints _waypoints;

    private EnemyWave _currentWave;

    private void OnEnable()
    {
        GameManager.Instance.OnWaveStarted += StartWave;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnWaveStarted -= StartWave;
    }

    private void StartWave()
    {
        _currentWave = _waves[GameManager.Instance.GetCurrentWave()];
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        EnemyWave currentWave = _currentWave;

        for (int i = 0; i < currentWave.EnemyTypesInWave.Length; i++)
        {
            for (int j = 0; j < currentWave.NumberOfEnemiesPerType[i]; j++)
            {
                //GameObject enemy = Instantiate(currentWave.EnemyTypesInWave[i], _spawnPoint.position, _spawnPoint.rotation);
                //enemy.GetComponent<EnemyController>().SetWaypointsAndSpawnPoint(_waypoints, _spawnPoint);
                //enemy.SendMessage("TheStart", _waypoints);

                yield return new WaitForSeconds(currentWave.TimeBetweenEnemies); // time to wait between enemies spawning
            }
        }
    }
}
