using Firebase.Auth;
using UI;
using UnityEngine;

namespace Core
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuGroup;
        [SerializeField] private GameObject accountMenuGroup;
        [SerializeField] private GameObject journalGroup;

        private GameManager gameManager;
        private JournalManager journalManager;
        private FirebaseAuth auth;
        
        private JournalUI journalUI;

        private void Awake()
        {
            gameManager = GameManager.Instance;
            journalManager = gameManager.gameObject.GetComponent<JournalManager>();
            auth = FirebaseAuth.DefaultInstance;

            journalUI = GetComponent<JournalUI>();

            if (auth.CurrentUser == null)
                ShowAccountMenu();
            else
                gameManager.userID = auth.CurrentUser.UserId;
        }

        private void OnEnable()
        {
            AccountManager.OnLoginSuccessAction += journalManager.InitializeCaptureData;
            JournalManager.OnFetchSuccessAction += journalUI.PopulateJournal;
        }

        private void OnDisable()
        {
            AccountManager.OnLoginSuccessAction -= journalManager.InitializeCaptureData;
            JournalManager.OnFetchSuccessAction -= journalUI.PopulateJournal;
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