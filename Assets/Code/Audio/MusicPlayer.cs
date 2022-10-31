using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioPlayer))]
public class MusicPlayer : MonoBehaviour
{
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Start()
    {
        _audioPlayer.ConfigureAudioSource("Music");
    }

    public void PlaySong(string songName)
    {
        _audioPlayer.PlayAudio(songName);
    }
}
