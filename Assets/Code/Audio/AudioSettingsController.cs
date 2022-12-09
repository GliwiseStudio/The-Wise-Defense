using UnityEngine;

public class AudioSettingsController : MonoBehaviour
{
    private AudioMixerGroupController _audioMixerGroupController;

    private void Awake()
    {
        _audioMixerGroupController = FindObjectOfType<AudioMixerGroupController>();
    }

    private void Start()
    {
        ChangeAudioMixerParameter("Master", "MasterVolume", 1f);
        ChangeAudioMixerParameter("UIEffects", "UIVolume", 1f);
        ChangeAudioMixerParameter("Music", "MusicVolume", 1f);
        ChangeAudioMixerParameter("SoundEffects", "SoundEffectsVolume", 1f);
    }

    public void ChangeAudioMixerParameter(string audioMixerName, string parameterName, float value)
    {
        _audioMixerGroupController.SetAudioMixerVolume(audioMixerName, parameterName, value);
    }

    public float GetParameterFromAudioMixer(string audioMixerName, string parameterName)
    {
        return _audioMixerGroupController.GetAudioMixerVolume(audioMixerName, parameterName);
    }
}
