using UnityEngine;

public class PlayFabDataRetrieverSubstitute : MonoBehaviour, IDataRetriever
{
    [SerializeField] private int _levelIndex = 1;

    public int GetLastLevelIndex()
    {
        return _levelIndex;
    }
}
