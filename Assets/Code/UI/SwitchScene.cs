using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(GoToScene);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(GoToScene);
    }
    private void GoToScene()
    {
        Time.timeScale = 1; // leaving the pause menu, so unpause
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }
}
