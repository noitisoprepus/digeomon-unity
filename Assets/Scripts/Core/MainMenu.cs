using Core;
using Firebase.Auth;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        private FirebaseAuth auth;

        private void Awake()
        {
            auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser == null)
                LoadAccount();
            else
                GameManager.Instance.userID = auth.CurrentUser.UserId;
        }

        public void OnPlayButtonPressed()
        {
            GameManager.Instance.GoToScene("Scanner");
        }

        public void OnLogoutButtonPressed()
        {
            auth.SignOut();
            LoadAccount();
        }

        private void LoadAccount()
        {
            GameManager.Instance.GoToScene("Account");
        }
    }
}