using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level/Level Configuration Storage", fileName = "LevelConfigurationsStorage")]
public class LevelConfigurationStorage : ScriptableObject
{
    [SerializeField] private List<LevelConfigurationSO> _levelConfigurations;

    public LevelConfigurationSO GetLevelConfigurationFromIndex(int index)
    {
        return _levelConfigurations[index];
    }
}
