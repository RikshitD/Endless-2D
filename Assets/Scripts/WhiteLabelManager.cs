using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine.SceneManagement;

public class WhiteLabelManager : MonoBehaviour
{
    private string gameSceneName = "Menu";
    public TMP_InputField newUserEmailInputField;
    public TMP_InputField newUserPasswordInputField;
    public TMP_InputField nickNameInputField;
    public TMP_InputField existingUserEmailInputField;
    public TMP_InputField existingUserPasswordInputField;
    public CanvasAnimator loginCanvasAnimator;
    public TMP_InputField resetPasswordInputField;
    public Toggle rememberMeToggle;
    public Animator rememberMeAnimator;
    public Animator autoLoginButtonAnimator;
    public Animator loginButtonAnimator;
    public Animator backButtonAnimator;
    public Animator loginBackButtonAnimator;
    public Animator newUserButtonAnimator;
    public Animator resetPasswordButtonAnimator;
    public TextMeshProUGUI playerNameText;
    private static WhiteLabelManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        int rememberMe = PlayerPrefs.GetInt("rememberMe", 0);
        rememberMeToggle.isOn = Convert.ToBoolean(rememberMe);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void Login()
    {
        string email = existingUserEmailInputField.text;
        string password = existingUserPasswordInputField.text;
        LootLockerSDKManager.WhiteLabelLogin(email, password, Convert.ToBoolean(rememberMeToggle.isOn), response =>
        {
            if (!response.success)
            {
                loginButtonAnimator.SetTrigger("Error");
                backButtonAnimator.SetTrigger("Show");
                return;
            }

            if (response.VerifiedAt == null)
            {
  
            }

            LootLockerSDKManager.StartWhiteLabelSession((response) =>
            {
                if (!response.success)
                {
                    loginButtonAnimator.SetTrigger("Error");
                    backButtonAnimator.SetTrigger("Show");
                    return;
                }
                else
                {
                    loginButtonAnimator.SetTrigger("LoggedIn");
                    backButtonAnimator.SetTrigger("Show");

                    PlayerPrefs.SetInt("player_id", response.player_id);

                    PlayGame();
                }
            });
        });
    }

    private void SetPlayerNameToGameScreen()
{
    LootLockerSDKManager.GetPlayerName((response) =>
    {
        if (response.success)
        {
            TextMeshProUGUI playerNameText = GetComponentInChildren<TextMeshProUGUI>();

            playerNameText.text = response.name;
        }
    });
}


    public void NewUser()
    {
        if (newUserEmailInputField == null || newUserPasswordInputField == null)
        {
            Debug.LogError("User email or password input field is not assigned.");
            return;
        }

        string email = newUserEmailInputField.text;
        string password = newUserPasswordInputField.text;
        string newNickName = nickNameInputField != null ? nickNameInputField.text : "";

        void Error(string error)
        {
            Debug.Log(error);
            newUserButtonAnimator.SetTrigger("Error");
            backButtonAnimator.SetTrigger("Show");
        }

        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (!response.success)
            {
                Error(response.Error);
                return;
            }
            else
            {
                LootLockerSDKManager.WhiteLabelLogin(email, password, false, response =>
                {
                    if (!response.success)
                    {
                        Error(response.Error);
                        return;
                    }

                    LootLockerSDKManager.StartWhiteLabelSession((response) =>
                    {
                        if (!response.success)
                        {
                            Error(response.Error);
                            return;
                        }

                        if (string.IsNullOrEmpty(newNickName))
                        {
                            newNickName = response.public_uid;
                        }

                        LootLockerSDKManager.SetPlayerName(newNickName, (response) =>
                        {
                            if (!response.success)
                            {
                                Error(response.Error);
                                return;
                            }

                            LootLockerSessionRequest sessionRequest = new LootLockerSessionRequest();
                            LootLocker.LootLockerAPIManager.EndSession(sessionRequest, (response) =>
                            {
                                if (!response.success)
                                {
                                    Error(response.Error);
                                    return;
                                }
                                Debug.Log("Account Created");
                                newUserButtonAnimator.SetTrigger("AccountCreated");
                                backButtonAnimator.SetTrigger("Show");
                                rememberMeToggle.isOn = false;
                            });
                        });
                    });
                });
            }
        });
    }

    public void ToggleRememberMe()
    {
        bool rememberMeBool = rememberMeToggle.isOn;
        rememberMeAnimator.SetTrigger(rememberMeBool ? "On" : "Off");
        PlayerPrefs.SetInt("rememberMe", rememberMeBool ? 1 : 0);
    }

    public void AutoLogin()
    {
        if (Convert.ToBoolean(PlayerPrefs.GetInt("rememberMe", 0)))
        {
            existingUserEmailInputField.GetComponent<Animator>().ResetTrigger("Show");
            existingUserEmailInputField.GetComponent<Animator>().SetTrigger("Hide");
            existingUserEmailInputField.GetComponent<Animator>().ResetTrigger("Show");
            existingUserPasswordInputField.GetComponent<Animator>().SetTrigger("Hide");
            loginBackButtonAnimator.ResetTrigger("Show");
            loginBackButtonAnimator.SetTrigger("Hide");
            loginButtonAnimator.ResetTrigger("Hide");
            loginButtonAnimator.SetTrigger("Show");
            loginButtonAnimator.SetTrigger("Login");

            LootLockerSDKManager.CheckWhiteLabelSession(response =>
            {
                if (!response)
                {
                    loginButtonAnimator.SetTrigger("Error");
                    backButtonAnimator.SetTrigger("Show");
                    rememberMeToggle.isOn = false;
                }
                else
                {
                    LootLockerSDKManager.StartWhiteLabelSession((response) =>
                    {
                        if (response.success)
                        {
                            loginButtonAnimator.SetTrigger("LoggedIn");
                            backButtonAnimator.SetTrigger("Show");
                            PlayGame();
                        }
                        else
                        {
                            loginButtonAnimator.SetTrigger("Error");
                            backButtonAnimator.SetTrigger("Show");
                            rememberMeToggle.isOn = false;
                            return;
                        }
                    });

                }

            });
        }
        else
        {
            loginCanvasAnimator.CallAppearOnAllAnimators();
        }
    }

    public void PasswordReset()
    {
        string email = resetPasswordInputField.text;
        LootLockerSDKManager.WhiteLabelRequestPassword(email, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("Error requesting password reset");
                resetPasswordButtonAnimator.SetTrigger("Error");
                backButtonAnimator.SetTrigger("Show");
                return;
            }

            Debug.Log("Requested password reset successfully");
            resetPasswordButtonAnimator.SetTrigger("Done");
            backButtonAnimator.SetTrigger("Show");
        });
    }

    public void ResendVerificationEmail()
    {
        int playerID = 0;
        LootLockerSDKManager.WhiteLabelRequestVerification(playerID, (response) =>
        {
            if (response.success)
            {
                // Email was sent
            }
        });
    }
}
