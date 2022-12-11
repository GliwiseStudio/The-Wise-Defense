using UnityEngine;

public class UIInGameScreen : MonoBehaviour, UIScreen
{
    [SerializeField] private string _name;
    private bool _firstTime = true;

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

        if (_firstTime)
        {
            _firstTime = false;
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = FindObjectOfType<IncreaseWaveSpeed>().GetSpeed();
        }
        
    }
}
