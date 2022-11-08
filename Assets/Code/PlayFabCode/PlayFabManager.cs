using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    private static PlayFabManager _instance;

    // Player related variables
    private static int _numberOfLevels = 15;
    public List<Level> UnlockedLevels = new List<Level>();
    public int LastUnlockedLevel = 0;
    public int CurrentLevel = 0;

    private bool _firstDataGotten = false; // to go to MainMenu once the player has gotten the data
                                           // from PlayFab when login in, because it takes a second or so
    public static PlayFabManager Instance
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

    public void GetCurrentLevel(int currentLevel)
    {
        CurrentLevel = currentLevel;
    }

    #region Get/send data from/to PlayFab
    public void GetUnlockedLevels()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    public void SendUnlockedLevels()
    {
        UpdateUserDataRequest request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"UnlockedLevels", JsonConvert.SerializeObject(UnlockedLevels)},
                {"LastUnlockedLevel", LastUnlockedLevel.ToString() }
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
        if (result.Data != null && result.Data.ContainsKey("UnlockedLevels"))
        {
            UnlockedLevels = JsonConvert.DeserializeObject<List<Level>>(result.Data["UnlockedLevels"].Value);
            LastUnlockedLevel = int.Parse(result.Data["LastUnlockedLevel"].Value);
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
