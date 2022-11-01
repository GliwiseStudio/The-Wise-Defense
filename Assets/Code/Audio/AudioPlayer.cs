using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioController _audioController;
    private AudioMixerGroupController _audioMixerGroupController;

    private void Awake()
    {
        _audioController = FindObjectOfType<AudioController>();
        _audioMixerGroupController = FindObjectOfType<AudioMixerGroupController>();

        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ConfigureAudioSource("Master");
    }

    public void ConfigureAudioSource(string audioMixerName = "Master")
    {
        _audioSource.playOnAwake = false;
        _audioSource.outputAudioMixerGroup = _audioMixerGroupController.GetAudioMixerGroupFromName(audioMixerName);
    }

    public void PlayAudio(string audioName)
    {
        _audioSource.clip = _audioController.GetAudioClipFromName(audioName);
        _audioSource.Play();
    }
}
