using System;
using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
 
    public event Action OnWin;
    public event Action OnLoose;
    
    public event Action OnWaveFinished;
    public event Action OnWaveStarted;

    [SerializeField] private int _numWaves = 5;
    
    private int _currentWave = 0;
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
        _instance = this;
        PauseGame();
    }

    #region Win / Loose
    private void Win()
    {
        Debug.Log("Game's over and you win !! :D");
        UpgradePlayFabInfo();
        OnWin?.Invoke();
    }

    private void Loose()
    {
        Debug.Log("Game ends and you loose :(");
        OnLoose?.Invoke();
    }

    #endregion

    #region Manage game data to send to playfab
    private void UpgradePlayFabInfo()
    {
        if (PlayFabManager.Instance != null) // it won't be null on build, but it's possible that it will be while tryhing levels on unity
        {
            bool _needToUpdateSavedData = false;
            int currentLevel = PlayFabManager.Instance.CurrentLevel;

            if (PlayFabManager.Instance.UnlockedLevels[currentLevel + 1].unlocked == false)
            {
                PlayFabManager.Instance.UnlockedLevels[currentLevel + 1].unlocked = true; // new unlocked level
                _needToUpdateSavedData = true;
            }

            if (PlayFabManager.Instance.UnlockedLevels[currentLevel].stars < _currentTowers)
            {
                PlayFabManager.Instance.UnlockedLevels[currentLevel].stars = _currentTowers; // new stars record
                _needToUpdateSavedData = true;
            }

            if (_needToUpdateSavedData)
            {
                PlayFabManager.Instance.SendUnlockedLevels();
            }
        }
        
    }

    #endregion

    #region Waves Management

    public void NextWave() // pause game and prepare for next wave, but not start it
    {
        OnWaveFinished?.Invoke();
        if (_currentWave < _numWaves)
        {
            PauseGame();
            _currentWave++;
            _currentEnemies = 0;
        }
        else
        {
            // There are no more waves left, player wins
            Win();
        }

    }

    public void StartWave() // called from the start button
    {
        OnWaveStarted?.Invoke();
        UnpauseGame();
    }

    #endregion

    #region Keep track of resources
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
            Loose();
        }
    }

    #endregion

    #region Pause/Unpause
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }
    #endregion

    #region Getters

    public int GetCurrentWave()
    {
        return _currentWave;
    }
    #endregion
}

