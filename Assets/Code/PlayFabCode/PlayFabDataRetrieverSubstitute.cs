using UnityEngine;

public class PlayFabDataRetrieverSubstitute : MonoBehaviour
{
    [SerializeField] private int _levelIndex = 1;

    public int GetLastLevelIndex()
    {
        if (LevelsManager.Instance != null)
        {
            return LevelsManager.Instance.GetLastUnlockedLevel();
        }
        else
        {
            return _levelIndex;
        }
    }
}
