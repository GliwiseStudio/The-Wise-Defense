using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : ScriptableObject
{
    [field: SerializeField]
    public GameObject[] EnemiesInWave { get; private set; }

    [field: SerializeField]
    public float TimeBeforeThisWave { get; private set; }

    [field: SerializeField]
    public float NumberOfEnemies { get; private set; }
}
