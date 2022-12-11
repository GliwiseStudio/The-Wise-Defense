using System;
using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public event Action OnEndScene; // to tell enemy that a scene has finished

    public event Action OnWaveFinished;
    public event Action OnWaveStarted;

    [SerializeField] private int _totalWaves = 10;
    [SerializeField] private int _currentTowers = 3;

    private int _currentWave = 0;
    private int _currentEnemies = 0;

    public static GameManager Instance
    {
        get
        {
            // not a problem because:
            // this happens when the gameManager is destroyed when loading a new screen, and other gameobjects being destroyed try to unsubscribe from it's events after it
            // but it is not a problem, because it is the event owner (gameManager) who is destroyed, so the other gameObjects don't really need to unsubscribe anyway
            // you should unsubscribe if you destroy the subscriber and the event owner lives on, because otherwise the delegates still point to the subscriber
            //if (_instance == null)
            //{
            //    Debug.Log("Game Manager is Null");
            //}
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void InvokeEndSceneEvent()
    {
        OnEndScene?.Invoke();
    }

    #region Win / Loose
    private void Win()
    {
        if (LevelsManager.Instance != null) // to show them on the game over screen
        {
            LevelsManager.Instance.SetCurrentStars(_currentTowers);
        }

        Debug.Log("Game's over and you win !! :D");

        UpgradePlayFabInfo(); // check if you need to upgrade playfab info

        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    private void Loose()
    {
        if (LevelsManager.Instance != null) // to show them on the game over screen
        {
            LevelsManager.Instance.SetCurrentStars(_currentTowers);
        }

        InvokeEndSceneEvent(); // to get rid of all remaining enemies

        Debug.Log("Game ends and you loose :(");

        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    #endregion

    #region Manage game levels data (and send to playfab if player is logged)
    private void UpgradePlayFabInfo()
    {
        if (LevelsManager.Instance != null) // it won't be null on build, but it's possible that it will be while tryhing levels on unity
        {
            bool _needToUpdateSavedData = false;
            int currentLevel = LevelsManager.Instance.GetCurrentLevel();
            Debug.Log("Current level number:" + currentLevel);
            Debug.Log("Count number: " + (LevelsManager.Instance.UnlockedLevels.Count - 1));
            if(currentLevel != (LevelsManager.Instance.UnlockedLevels.Count-1)) // if not on the last level
            {
                if (LevelsManager.Instance.UnlockedLevels[currentLevel + 1].unlocked == false)
                {
                    LevelsManager.Instance.UnlockedLevels[currentLevel + 1].unlocked = true; // new unlocked level
                    LevelsManager.Instance.UnlockedLevels[currentLevel + 1].newLevel = true;
                    //LevelsManager.Instance.SetLastUnlockedLevel(currentLevel + 1); // now this is done when the player has entered the new unlocked level
                    _needToUpdateSavedData = true;
                }
            }

            if (LevelsManager.Instance.UnlockedLevels[currentLevel].stars < _currentTowers)
            {
                LevelsManager.Instance.UnlockedLevels[currentLevel].stars = _currentTowers; // new stars record
                _needToUpdateSavedData = true;
            }

            if (_needToUpdateSavedData && !LevelsManager.Instance.GetIsPlayingAsGuest()) // if the player is not a guest, and the data has changed
            {
                LevelsManager.Instance.SendUnlockedLevelsToPlayfab();
            }
        }
    }

    #endregion

    #region Waves Management

    public void NextWave() // prepare for next wave, but not start it
    {
        if (_currentWave + 1 < _totalWaves)
        {
            _currentWave++;
            _currentEnemies = 0;
            OnWaveFinished?.Invoke();
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

        // check if there are any towers left, if there aren't the game is over
        if (_currentTowers == 0)
        {
            Loose();
        }
    }

    #endregion

    #region Pause/Unpause // deprecated
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }
    #endregion // deprecated

    #region Getters

    public int GetCurrentWave()
    {
        return _currentWave;
    }

    public int GetTotalWaves()
    {
        return _totalWaves;
    }
    #endregion
}

