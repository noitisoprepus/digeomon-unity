using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Core
{
    public class AccountManager : MonoBehaviour
    {
        GameManager gameManager;
        FirebaseAuth auth;
        DatabaseReference databaseReference;

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
            
            string databaseUri = gameManager.databaseUri;
            databaseReference = FirebaseDatabase.GetInstance(databaseUri).RootReference;
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
                UserData newUser = new UserData(username, new List<string>());
                string userDataJson = JsonUtility.ToJson(newUser);
                
                var databaseTask = databaseReference.Child("users").Child(user.UserId).SetRawJsonValueAsync(userDataJson);
                yield return new WaitUntil(() => databaseTask.IsCompleted);
                if (databaseTask.Exception != null)
                {
                    Debug.LogError("SetRawJsonValueAsync encountered an error: " + databaseTask.Exception);
                }
                else
                {
                    gameManager.GoToScene("Main Menu");
                }
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