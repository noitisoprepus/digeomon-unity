using Firebase.Auth;
using System.Collections;
using UI;
using UnityEngine;

namespace Core
{
    public class AccountManager : MonoBehaviour
    {
        private GameManager gameManager;
        private FirebaseAuth auth;

        private void OnEnable()
        {
            AccountUI.OnRegisterAction += HandleAccountRegistration;
            AccountUI.OnLoginAction += HandleAccountLogin;
        }

        private void OnDisable()
        {
            AccountUI.OnRegisterAction -= HandleAccountRegistration;
            AccountUI.OnLoginAction -= HandleAccountLogin;
        }

        private void Start()
        {
            gameManager = GameManager.Instance;

            auth = FirebaseAuth.DefaultInstance;
        }

        private void HandleAccountRegistration(string email, string password, string username)
        {
            StartCoroutine(CreateAccount(email, password, username));
        }

        private void HandleAccountLogin(string email, string password)
        {
            StartCoroutine(LoginAccount(email, password));
        }

        private IEnumerator CreateAccount(string email, string password, string username)
        {
            var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Exception != null)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            }
            else
            {
                // Firebase user has been created.
                AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
                StartCoroutine(CreateUserProfile(username));
            }
        }

        private IEnumerator CreateUserProfile(string username)
        {
            FirebaseUser user = auth.CurrentUser;
            UserProfile profile = new UserProfile
            {
                DisplayName = username,
            };
            var profileTask = user.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(() => profileTask.IsCompleted);

            if (profileTask.Exception != null)
            {
                Debug.LogError("UpdateUserProfileAsync encountered an error: " + profileTask.Exception);
            }
            else
            {
                gameManager.GoToScene("Main Menu");
            }
        }

        private IEnumerator LoginAccount(string email, string password)
        {
            var task = auth.SignInWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => task.IsCompleted);
            
            if (task.Exception != null)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            }
            else
            {
                AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

                gameManager.GoToScene("Main Menu");
            }
        }
    }
}