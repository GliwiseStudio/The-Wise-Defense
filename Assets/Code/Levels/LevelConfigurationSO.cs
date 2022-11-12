using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level/Level Configuration", fileName = "LevelXConfiguration")]
public class LevelConfigurationSO : ScriptableObject
{
    [SerializeField] private CardStorage _levelCardStorage;
    public CardStorage LevelCardStorage => _levelCardStorage;
}
