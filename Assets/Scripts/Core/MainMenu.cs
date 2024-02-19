using Core;
using Firebase.Auth;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuGroup;
        [SerializeField] private GameObject accountMenuGroup;
        [SerializeField] private GameObject journalGroup;

        private GameManager gameManager;
        private FirebaseAuth auth;

        private void Awake()
        {
            gameManager = GameManager.Instance;
            auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser == null)
                ShowAccountMenu();
            else
                gameManager.userID = auth.CurrentUser.UserId;
        }

        private void OnEnable()
        {
            JournalUI.OnJournalAction += gameManager.SyncCaptureData;
        }

        private void OnDisable()
        {
            JournalUI.OnJournalAction -= gameManager.SyncCaptureData;
        }

        public void MainMenuHome()
        {
            journalGroup.SetActive(false);
            accountMenuGroup.SetActive(false);
            mainMenuGroup.SetActive(true);
        }

        public void OnPlayButtonPressed()
        {
            gameManager.GoToScene("Scanner");
        }

        public void OnJournalButtonPressed()
        {
            mainMenuGroup.SetActive(false);
            journalGroup.SetActive(true);
        }

        public void OnLogoutButtonPressed()
        {
            auth.SignOut();
            ShowAccountMenu();
        }

        private void ShowAccountMenu()
        {
            mainMenuGroup.SetActive(false);
            accountMenuGroup.SetActive(true);
        }
    }
}