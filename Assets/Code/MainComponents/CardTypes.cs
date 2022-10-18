using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardTypes", menuName = "ScriptableObjects/CardTypes", order = 1)]
public class CardTypes : ScriptableObject
{
    [field: SerializeField]
    public CardConfigurationSO[] Cards { get; private set; }

    [field: SerializeField]
    public Sprite[] CardsSprite { get; private set; }
}

