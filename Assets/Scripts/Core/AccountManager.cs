using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using UI;
using UnityEngine;

namespace Core
{
    public class AccountManager : MonoBehaviour
    {
        public delegate void LoginSuccessDelegate();
        public static event LoginSuccessDelegate OnLoginSuccessAction;

        private FirebaseAuth auth;
        private DatabaseReference databaseReference;
        private MainMenuManager mainMenuManager;

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
            auth = FirebaseAuth.DefaultInstance;
            databaseReference = FirebaseDatabase.GetInstance(GameManager.Instance.databaseUri).GetReference("users");
            mainMenuManager = GetComponent<MainMenuManager>();
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

            databaseReference.Child(user.UserId).Child("username").SetValueAsync(username);

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
                OnLoginSuccessAction?.Invoke();
                mainMenuManager.MainMenuHome();
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

                OnLoginSuccessAction?.Invoke();
                mainMenuManager.MainMenuHome();
            }
        }
    }
}