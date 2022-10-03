using UnityEngine;

[CreateAssetMenu(fileName ="EnemyWave", menuName = "ScriptableObjects/EnemyWave", order = 1)]
public class EnemyWave : ScriptableObject
{
    [field: SerializeField]
    public GameObject[] EnemiesInWave { get; private set; }

    [field: SerializeField]
    public float TimeBeforeThisWave { get; private set; }
}