using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartLevel : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ReloadScene);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ReloadScene);
    }

    private void ReloadScene()
    {
        GameManager.Instance.InvokeEndSceneEvent(); // to get rid of the remaining enemies

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }
}
