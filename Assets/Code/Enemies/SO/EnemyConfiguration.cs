using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "ScriptableObjects/EnemyConfiguration", order = 1)]
public class EnemyConfiguration : ScriptableObject
{
    [field: SerializeField] 
    public float Speed { get; private set; }

    [field: SerializeField]
    public float MaxHealth { get; private set; }

    [field: SerializeField]
    public float DetectionRange { get; private set; }

    [field: SerializeField]
    public int Damage { get; private set; }

    [field: SerializeField]
    public float DamageAnimTime { get; private set; }

    [field: SerializeField]
    public float HitTime { get; private set; }
}
