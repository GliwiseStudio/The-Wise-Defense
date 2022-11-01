using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private int _levelNumber;
    [SerializeField] private string _levelSceneName;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private Image _starsImage;
    [SerializeField] private Sprite[] _starSprites;
    private bool _unlocked;


    private void Start()
    {
        _unlocked = PlayFabManager.Instance.UnlockedLevels[_levelNumber].unlocked;

        SetLevelState();
    }

    private void SetLevelState()
    {
        if (_unlocked)
        {
            _lockedImage.gameObject.SetActive(false);
            _starsImage.gameObject.SetActive(true);

            int starsUnlocked = PlayFabManager.Instance.UnlockedLevels[_levelNumber].stars;
            switch (starsUnlocked)
            {
                case 0:
                    _starsImage.sprite = _starSprites[0];
                    break;
                case 1:
                    _starsImage.sprite = _starSprites[1];
                    break;
                case 2:
                    _starsImage.sprite = _starSprites[2];
                    break;
                case 3:
                    _starsImage.sprite = _starSprites[3];
                    break;
            }

        }
    }

    public void GoToLevel()
    {
        PlayFabManager.Instance.GetCurrentLevel(_levelNumber);
        SceneManager.LoadScene(_levelSceneName, LoadSceneMode.Single);
    }
}
