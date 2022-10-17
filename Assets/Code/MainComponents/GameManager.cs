using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private int _numWaves = 5;

    private int _currentWave = 0;
    private bool _isWaveActive = false;
    private int _currentEnemies = 0;
    private int _currentTowers = 3;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Game Manager is Null!! Fix this!! ");
            return _instance;
        }
    }

    private void Awake()
    {
        PauseGame();
        _instance = this;
    }

    #region Waves Management
    public void NextWave() // pause game and prepare for next wave, but not start it
    {
        if (_currentWave < _numWaves)
        {
            PauseGame();
            _currentWave++;
            _currentEnemies = 0;
        }
        else
        {
            Debug.Log("Game's over and you win !! :D");
        }

    }

    public void StartWave() // called from the start button
    {
        UnpauseGame();
    }

    #endregion

    #region Keep track of resources (wave over/win/lose condition)
    public void AddEnemy()
    {
        _currentEnemies++;
    }

    public void RemoveEnemy()
    {
        _currentEnemies--;

        // check if there are any enemies left, if there aren't the wave is over
        if (_currentEnemies == 0)
        {
            NextWave();
        }
    }

    public void RemoveTower()
    {
        _currentTowers--;

        // check if there are any enemies left, if there aren't the game is over
        if (_currentTowers == 0)
        {
            // end game
            Debug.Log("Game ends and you loose :(");
        }
    }

    #endregion

    #region Pause/Unpause
    public void PauseGame()
    {
        _isWaveActive = false;
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        _isWaveActive = true;
        Time.timeScale = 1;
    }
    #endregion

    #region Getters
    public bool GetIsWaveActive()
    {
        return _isWaveActive;
    }

    public int GetCurrentWave()
    {
        return _currentWave;
    }
    #endregion
}

