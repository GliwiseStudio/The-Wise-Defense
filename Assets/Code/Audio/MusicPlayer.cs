using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioPlayer))]
public class MusicPlayer : MonoBehaviour
{
    private AudioPlayer _audioPlayer;
    [SerializeField] private bool _callAutomatically = true;
    [SerializeField] private string _songName;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Start()
    {
        _audioPlayer.ConfigureAudioSource("Music", true);

        if (_callAutomatically)
        {
            PlaySong(_songName);
        }
    }

    public void PlaySong(string songName)
    {
        _audioPlayer.PlayAudio(songName);
    }
}
