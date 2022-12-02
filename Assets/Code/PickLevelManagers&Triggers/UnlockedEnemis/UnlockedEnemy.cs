using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemies/UnlockedEnemy", fileName = "UnlockedEnemy")]
public class UnlockedEnemy : ScriptableObject
{
    [SerializeField] private string _name;

    [SerializeField][TextArea] private string _description;

    [SerializeField] private Sprite _sprite;

    public string Name => _name;
    public string Description => _description;
    public Sprite Sprite => _sprite;
}
