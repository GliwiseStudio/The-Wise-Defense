using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerGroupController : MonoBehaviour
{
    [SerializeField] private AudioMixerGroupsStorageSO _audioMixerGroupStorage;

    private void Awake()
    {
        _audioMixerGroupStorage.Init();
    }

    public AudioMixerGroup GetAudioMixerGroupFromName(string name)
    {
        return _audioMixerGroupStorage.GetAudioMixerGroupFromName(name);
    }

    public void SetAudioMixerVolume(string name)
    {

    }
}
