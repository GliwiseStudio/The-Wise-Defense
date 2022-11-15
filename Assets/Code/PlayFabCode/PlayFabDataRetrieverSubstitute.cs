using UnityEngine;

public class PlayFabDataRetrieverSubstitute : MonoBehaviour
{
    [SerializeField] private int _levelIndex = 1;

    public int GetLastLevelIndex()
    {
        if (PlayFabManager.Instance != null)
        {
            return PlayFabManager.Instance.GetLastUnlockedLevel();
        }
        else
        {
            return _levelIndex;
        }
    }
}
