using System;
using UnityEngine;

[Serializable]
public class CardStatConfiguration
{
    [SerializeField] private CardStatType _name;
    [SerializeField] private float _value;

    public CardStatType Name => _name;
    public float Value => _value;
}
