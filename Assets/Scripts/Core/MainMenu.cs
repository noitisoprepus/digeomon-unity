using Core;
using Firebase.Auth;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuGroup;
        [SerializeField] private GameObject accountMenuGroup;

        private FirebaseAuth auth;

        private void Awake()
        {
            auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser == null)
                ShowAccountMenu();
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
            ShowAccountMenu();
        }

        public void OnLoginAccount()
        {
            accountMenuGroup.SetActive(false);
            mainMenuGroup.SetActive(true);
        }

        private void ShowAccountMenu()
        {
            mainMenuGroup.SetActive(false);
            accountMenuGroup.SetActive(true);
        }
    }
}