using System;
using UnityEngine;

[Serializable]
public class TowerAudioConfiguration
{
    [SerializeField] private string _shotSound;
    [SerializeField] private string _audioMixerChannel = "SoundEffects";

    public string ShotSound => _shotSound;
    public string AudioMixerChannel => _audioMixerChannel;
}
