using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Newtonsoft.Json;

public class PlayFabManager : MonoBehaviour
{
    private static PlayFabManager _instance;

    // Player related variables
    private static int _numberOfLevels = 15;
    public List<Level> UnlockedLevels = new List<Level>();
    public int CurrentLevel = 0;

    public bool FirstDataGotten = false; // now it's not used, but I've created it to remember
                                         // that in the future it should be used
                                         // to check if the game has gotten the player data from PlayFab
                                         // when login in, because it takes a second or so
                                         // and only go to the level selection screen if it has

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
        UnlockedLevels.Add(new Level(true, 0)); // the first level is always unlocked

        for (int i = 1; i < _numberOfLevels; i++) // the rest aren't
        {
            UnlockedLevels.Add(new Level(false, 0));
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
                {"UnlockedLevels", JsonConvert.SerializeObject(UnlockedLevels)}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    private void OnDataReceived(GetUserDataResult result)
    {
        Debug.Log("Recieved characters data!");
        if (result.Data != null && result.Data.ContainsKey("UnlockedLevels"))
        {
            UnlockedLevels = JsonConvert.DeserializeObject<List<Level>>(result.Data["UnlockedLevels"].Value);
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
    public int stars;

    public Level(bool unlocked, int stars)
    {
        this.unlocked = unlocked;
        this.stars = stars;
    }
}
