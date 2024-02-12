using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccountUI : MonoBehaviour
{
    public delegate void RegisterDelegate(string email, string password);
    public static event RegisterDelegate OnRegisterAction;

    public delegate void LoginDelegate(string email, string password);
    public static event LoginDelegate OnLoginAction;

    [Header("Login UI")]
    [SerializeField] private GameObject loginForm;
    [SerializeField] private TMP_InputField emailInputFieldL;
    [SerializeField] private TMP_InputField passwordInputFieldL;

    [Header("Registration UI")]
    [SerializeField] private GameObject registrationForm;
    [SerializeField] private TMP_InputField emailInputFieldR;
    [SerializeField] private TMP_InputField passwordInputFieldR;

    public void OnRegisterButtonPress()
    {
        OnRegisterAction?.Invoke(emailInputFieldR.text, passwordInputFieldR.text);
    }

    public void OnLoginButtonPress()
    {
        OnLoginAction?.Invoke(emailInputFieldL.text, passwordInputFieldL.text);
    }

    public void SwitchToRegistrationForm()
    {
        loginForm.SetActive(false);
        registrationForm.SetActive(true);
        Debug.Log("Register now");
    }

    public void SwitchToLoginForm()
    {
        loginForm.SetActive(true);
        registrationForm.SetActive(false);
        Debug.Log("Login now");
    }
}
