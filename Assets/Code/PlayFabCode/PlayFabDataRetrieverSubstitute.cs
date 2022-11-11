using UnityEngine;

public class PlayFabDataRetrieverSubstitute : MonoBehaviour
{
    [SerializeField] private int _levelIndex = 1;

    public int GetLastLevelIndex()
    {
        return _levelIndex;
    }
}
