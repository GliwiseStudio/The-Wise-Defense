using System;
using UnityEngine;

[Serializable]
public class IconConfiguration
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private CardStatType _name;

    public Sprite Sprite => _sprite;
    public CardStatType Name => _name;
}
