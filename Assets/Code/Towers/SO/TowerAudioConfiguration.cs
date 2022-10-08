using System;
using UnityEngine;

[Serializable]
public class TowerAudioConfiguration
{
    [SerializeField] private string _shotSound;

    public string ShotSound => _shotSound;
}
