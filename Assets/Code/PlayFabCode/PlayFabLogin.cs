using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

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
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "40A3E"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset mail sent!";
    }

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
    }

    void OnRegisterSucces(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and logged in!";

        LevelsManager.Instance.InitializeLevels();
        LevelsManager.Instance.SendUnlockedLevelsToPlayfab();

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged in :) Loading player data... ";

        LevelsManager.Instance.SetPlayerLogged(true); // player has succesfully logged in
        LevelsManager.Instance.GetUnlockedLevelsFromPlayfab();
    }
}
