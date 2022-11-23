using UnityEngine;

public class UIInGameScreen : MonoBehaviour, UIScreen
{
    [SerializeField] private string _name;

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
        gameObject.SetActive(true);

        Time.timeScale = 1;
    }
}
