using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPoolSO", menuName = "ScriptableObjects/Enemies/EnemyPool", order = 1)]
public class EnemyPoolSO : ScriptableObject
{
    [field: SerializeField]
    public RecyclableObject EnemyRecyclableObject { get; private set; }

    [field: SerializeField]
    public int MaxPoolObjects { get; private set; }

}
