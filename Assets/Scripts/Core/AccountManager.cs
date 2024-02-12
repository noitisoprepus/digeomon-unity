using Firebase.Auth;
using UnityEngine;

namespace Core
{
    public class AccountManager : MonoBehaviour
    {
        FirebaseAuth auth;

        private void OnEnable()
        {
            AccountUI.OnRegisterAction += CreateAccount;
            AccountUI.OnLoginAction += LoginAccount;
        }

        private void OnDisable()
        {
            AccountUI.OnRegisterAction -= CreateAccount;
            AccountUI.OnLoginAction -= LoginAccount;
        }

        private void Start()
        {
            auth = FirebaseAuth.DefaultInstance;
        }

        private void CreateAccount(string email, string password)
        {
            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
            });
        }

        private void LoginAccount(string email, string password)
        {
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
            });
        }
    }
}