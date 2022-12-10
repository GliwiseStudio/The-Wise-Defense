using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaveGame : MonoBehaviour
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
        GameManager.Instance.InvokeEndSceneEvent(); // to get rid of the remaining enemies

        Time.timeScale = 1; // leaving the pause menu, so unpause
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }
}
