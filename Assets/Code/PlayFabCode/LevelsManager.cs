using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class LevelsManager: MonoBehaviour
{
    private static LevelsManager _instance;

    // Registered or not
    private bool _playerLogged;

    // Player related variables
    private static int _numberOfLevels = 15;
    public List<Level> UnlockedLevels = new List<Level>();
    private int _lastUnlockedLevel = 0;
    private int _currentLevel = 0;
    private int _currentStars = 0;

    private bool _firstDataGotten = false; // to go to MainMenu once the player has gotten the data
                                           // from PlayFab when login in, because it takes a second or so
    public static LevelsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // this will happen if there is no instance of playfab manager
                // but because it is initiliaced on the first scene, and never destroyed
                // this will only happen if you play a scene without following the 
                // natural order of the build, hence, why shouldn't happen on the build, don't worry
                Debug.Log("PlayFabManager is null, don't worry, this error won't be on the build :)");
                return null;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeLevels() // initialize levels for new players
    {
        UnlockedLevels.Add(new Level(true, true, 0)); // the first level is always unlocked

        for (int i = 1; i < _numberOfLevels; i++) // the rest aren't
        {
            UnlockedLevels.Add(new Level(false, false, 0));
        }
    }

    #region Getters/setters

    public void SetCurrentLevel(int currentLevel)
    {
        _currentLevel = currentLevel;
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public void SetLastUnlockedLevel(int lastUnlockedLevel)
    {
        _lastUnlockedLevel = lastUnlockedLevel;
    }

    public int GetLastUnlockedLevel()
    {
        return _lastUnlockedLevel;
    }

    public int GetCurrentStars()
    {
        return _currentStars;
    }

    public void SetCurrentStars(int currentStars)
    {
        _currentStars = currentStars;
    }

    public bool GetPlayerLogged()
    {
        return _playerLogged;
    }

    public void SetPlayerLogged(bool playerLogged)
    {
        _playerLogged = playerLogged;
    }
    #endregion

    #region Get/send data from/to PlayFab
    public void GetUnlockedLevelsFromPlayfab()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    public void SendUnlockedLevelsToPlayfab()
    {
        UpdateUserDataRequest request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"UnlockedLevels", JsonConvert.SerializeObject(UnlockedLevels)},
                {"LastUnlockedLevel", _lastUnlockedLevel.ToString() }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    private void OnDataReceived(GetUserDataResult result)
    {
        if(_firstDataGotten == false)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            _firstDataGotten = true;
        }
        Debug.Log("Recieved characters data!");
        if (result.Data != null && result.Data.ContainsKey("UnlockedLevels") && result.Data.ContainsKey("LastUnlockedLevel"))
        {
            UnlockedLevels = JsonConvert.DeserializeObject<List<Level>>(result.Data["UnlockedLevels"].Value);
            _lastUnlockedLevel = int.Parse(result.Data["LastUnlockedLevel"].Value);
        }
        else
        {
            InitializeLevels();
            SendUnlockedLevelsToPlayfab();
        }
    }

    private void OnDataSend(UpdateUserDataResult obj)
    {
        Debug.Log("data sended");
    }

    private void OnError(PlayFabError obj)
    {
        Debug.Log("there was an error sending the data");
    }

    #endregion
}

public class Level
{
    public bool unlocked;
    public bool newLevel;
    public int stars;

    public Level(bool unlocked, bool newLevel, int stars)
    {
        this.unlocked = unlocked;
        this.newLevel = newLevel;
        this.stars = stars;
    }
}
