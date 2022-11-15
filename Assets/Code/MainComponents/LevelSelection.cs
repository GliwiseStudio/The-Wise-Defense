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

            if (!PlayFabManager.Instance.UnlockedLevels[_levelNumber].newLevel)
            {
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
    }

    public void OnClick()
    {
        if (_unlocked)
        {
            if (PlayFabManager.Instance.UnlockedLevels[_levelNumber].newLevel) // clicked in a new level for the first time
            {
                PlayFabManager.Instance.UnlockedLevels[_levelNumber].newLevel = false; // no longer a new level
                PlayFabManager.Instance.SendUnlockedLevels(); // send that information

                // if the level has dialogue trigger the dialogue
                DialogueTrigger _levelDialogue = gameObject.GetComponent<DialogueTrigger>();
                if (_levelDialogue != null)
                {
                    _levelDialogue.TriggerDialogue(this);
                }
                else // there was no dialogue, but maybe there are new unlocked cards
                {
                    // if the level has new unlocked cards
                    UnlockedCardsTrigger _levelUnlockedCards = gameObject.GetComponent<UnlockedCardsTrigger>();
                    if (_levelUnlockedCards != null)
                    {
                        _levelUnlockedCards.TriggerUnlockedCards(this);
                    }
                }
            }
            else // already unlocked the level previously, go to level straight away
            {
                GoToLevel();
            }
        }
    }

    public void ShowCardsNext()
    {
        // if the level has new unlocked cards
        UnlockedCardsTrigger _levelUnlockedCards = gameObject.GetComponent<UnlockedCardsTrigger>();
        if (_levelUnlockedCards != null)
        {
            _levelUnlockedCards.TriggerUnlockedCards(this);
        }
        else
        {
            GoToLevel();
        }
    }

    public void GoToLevel()
    {
        PlayFabManager.Instance.SetCurrentLevel(_levelNumber);
        SceneManager.LoadScene(_levelSceneName, LoadSceneMode.Single);
    }
}
