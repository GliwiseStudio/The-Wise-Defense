using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayAsGuestBtn : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(PlayAsGuest);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(PlayAsGuest);
    }
    private void GoToScene()
    {
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }

    private void PlayAsGuest()
    {
        LevelsManager.Instance.SetIsPlayingAsGuest(true); // player hasn't logged in, he's playing as a guest
        LevelsManager.Instance.InitializeLevels(); // create the levels for the first time

        GoToScene(); // go to game screen
    }
}
