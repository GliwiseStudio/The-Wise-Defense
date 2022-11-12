using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class WebLinks : MonoBehaviour
{
    private Button _button;
    private UIScreenController _screenController;
    public string links;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _screenController = FindObjectOfType<UIScreenController>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ButtonLinks);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ButtonLinks);
    }
    public void ButtonLinks()
    {
        Application.OpenURL(links);
        Debug.Log("Me he metido" + links);
    }
}
