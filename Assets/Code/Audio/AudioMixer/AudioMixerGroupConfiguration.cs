using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class AudioMixerGroupConfiguration
{
    [SerializeField] private string _name;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;

    public string Name => _name;
    public AudioMixerGroup AudioMixerGroup => _audioMixerGroup;
}
