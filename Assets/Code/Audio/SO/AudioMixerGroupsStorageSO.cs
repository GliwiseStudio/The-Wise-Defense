using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "ScriptableObjects/Audio/Audio Mixer Groups Storage", fileName = "AudioMixerGroupsStorage")]
public class AudioMixerGroupsStorageSO : ScriptableObject
{
    [SerializeField] private List<AudioMixerGroupConfiguration> _audioMixerGroups;
    private IDictionary<string, AudioMixerGroup> _audioMixerGroupstorage;
    private bool _isInitialized = false;

    public void Init()
    {
        InitStorage();
        _isInitialized = true;
    }

    private void InitStorage()
    {
        _audioMixerGroupstorage = new Dictionary<string, AudioMixerGroup>();
        bool status = true;

        foreach (AudioMixerGroupConfiguration audioMixerGroup in _audioMixerGroups)
        {
            status = _audioMixerGroupstorage.TryAdd(audioMixerGroup.Name, audioMixerGroup.AudioMixerGroup);

#if UNITY_EDITOR
            if (!status)
            {
                Debug.LogWarning($"The are more than one audio mixer group called {audioMixerGroup.Name}.");
            }
#endif
        }
    }

    public AudioMixerGroup GetAudioMixerGroupFromName(string name)
    {
        Assert.IsTrue(_isInitialized, $"[AudioMixerGroupsStorageSO at GetAudioMixerGroupFromName]: The audioMixerGroupStorage needs to be initialized. Call Init() method");

        AudioMixerGroup wantedAudioMixerGroup;
        bool status = _audioMixerGroupstorage.TryGetValue(name, out wantedAudioMixerGroup);

        Assert.IsTrue(status, $"[AudioMixerGroupsStorageSO at GetAudioMixerGroupFromName]: There is not an audio mixer group called {name} in the storage.");

        return wantedAudioMixerGroup;
    }
}
