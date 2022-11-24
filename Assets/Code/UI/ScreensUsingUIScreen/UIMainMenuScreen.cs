using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UIMainMenuScreen : MonoBehaviour, UIScreen
{
    [SerializeField] private string _name;
    [SerializeField] private Button _registerBtn;

    private MusicPlayer _musicPlayer;

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public string GetName()
    {
        return _name;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        if (LevelsManager.Instance != null && LevelsManager.Instance.GetIsPlayingAsGuest())
        {
            _registerBtn.gameObject.SetActive(true);
        }
        else
        {
            _registerBtn.gameObject.SetActive(false);
        }

        gameObject.SetActive(true);
    }
}
