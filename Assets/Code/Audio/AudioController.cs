using UnityEngine;

[DisallowMultipleComponent]
public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioStorageSO _audioStorage;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _audioStorage.Init();
    }

    public AudioClip GetAudioClipFromName(string name)
    {
        return _audioStorage.GetAudioClipFromName(name);
    }
}
