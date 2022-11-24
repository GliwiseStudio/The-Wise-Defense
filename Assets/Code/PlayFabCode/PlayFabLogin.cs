using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    [SerializeField] private Button _backBtn;

    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password too short, it must have at least six characters";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucces, OnError);

        messageText.text = "Registering player...";

        // disable button, player cannot leave while loading data
        _backBtn.interactable = false;
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);

        messageText.text = "Loading player...";

        // disable button, player cannot leave while loading data
        _backBtn.interactable = false;
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "40A3E"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);

        messageText.text = "Sending email...";

        // disable button, player cannot leave while loading data
        _backBtn.interactable = false;
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset email sent!";

        // enable button, player can leave if it wants to
        _backBtn.interactable = true;
    }

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;

        // enable button, player can leave if it wants to
        _backBtn.interactable = true;
    }

    void OnRegisterSucces(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and logged in!";

        if (!LevelsManager.Instance.GetIsPlayingAsGuest()) // hasn't already created the levels
        {
            LevelsManager.Instance.InitializeLevels();
        }
        LevelsManager.Instance.SendUnlockedLevelsToPlayfab();

        if (LevelsManager.Instance.GetIsPlayingAsGuest())
        {
            FindObjectOfType<UIScreenController>().SwitchToPreviousScreen();
        }
        else
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }

    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged in :) Loading player data... ";

        LevelsManager.Instance.SetIsPlayingAsGuest(false); // player has succesfully logged in
        LevelsManager.Instance.GetUnlockedLevelsFromPlayfab();
    }
}
