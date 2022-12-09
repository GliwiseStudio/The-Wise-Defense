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

    public void SetAudioMixerVolume(string groupName, string parameterName, float value)
    {
        GetAudioMixerGroupFromName(groupName).audioMixer.SetFloat(parameterName, Mathf.Log10(value) * 20);
    }

    public float GetAudioMixerVolume(string groupName, string parameterName)
    {
        float db = -1f;
        GetAudioMixerGroupFromName(groupName).audioMixer.GetFloat(parameterName, out db);

        float result = Mathf.Pow(10, db / 20);
        return result;
    }
}
