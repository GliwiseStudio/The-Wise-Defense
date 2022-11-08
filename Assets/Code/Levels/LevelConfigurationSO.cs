using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level/Level Configuration", fileName = "LevelXConfiguration")]
public class LevelConfigurationSO : ScriptableObject
{
    [SerializeField] private CardsStorage _levelCardStorage;
    public CardsStorage LevelCardStorage => _levelCardStorage;
}
