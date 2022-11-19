using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Image _starsImage;
    [SerializeField] private Sprite[] _starSprites;
    [SerializeField] private GameObject _winGamePanel;
    [SerializeField] private GameObject _looseGamePanel;
    [SerializeField] private Animator _kingAnimator;
    private MusicPlayer _musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _musicPlayer = FindObjectOfType<MusicPlayer>();
        int starsUnlocked = PlayFabManager.Instance.GetCurrentStars();

        if (starsUnlocked == 0) // Lost game
        {
            _musicPlayer.PlaySong("DefeatMusic");
            _looseGamePanel.SetActive(true);
            _winGamePanel.SetActive(false);
            _kingAnimator.Play("KingCrying");
        }
        else // Won game
        {
            _musicPlayer.PlaySong("VictoryMusic");
            _looseGamePanel.SetActive(false);
            _winGamePanel.SetActive(true);
            _kingAnimator.Play("KingVictory");

            if (PlayFabManager.Instance.GetCurrentLevel() == 0)
            {
                starsUnlocked = 3; // Because the tutorial only has one tower
            }

            switch (starsUnlocked)
            {
                case 1:
                    _starsImage.sprite = _starSprites[0];
                    break;
                case 2:
                    _starsImage.sprite = _starSprites[1];
                    break;
                case 3:
                    _starsImage.sprite = _starSprites[2];
                    break;
            }
        }
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("PickLevel", LoadSceneMode.Single);
    }
}
