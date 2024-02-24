using Firebase.Auth;
using UI;
using UnityEngine;

namespace Core
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuGroup;
        [SerializeField] private GameObject accountMenuGroup;

        private JournalManager journalManager;
        private FirebaseAuth auth;
        
        private JournalUI journalUI;
        private LeaderboardsUI leaderboardsUI;

        private void Awake()
        {
            journalManager = GameManager.Instance.gameObject.GetComponent<JournalManager>();
            auth = FirebaseAuth.DefaultInstance;

            journalUI = GetComponent<JournalUI>();
            leaderboardsUI = GetComponent<LeaderboardsUI>();

            if (auth.CurrentUser == null)
                ShowAccountMenu();
            else
            {
                if (!PlayerPrefs.HasKey("userID"))
                    PlayerPrefs.SetString("userID", auth.CurrentUser.UserId);
                journalManager.InitializeCaptureData();
                MainMenuHome();
            }
        }

        private void OnEnable()
        {
            AccountManager.OnLoginSuccessAction += journalManager.InitializeCaptureData;
            JournalManager.OnFetchSuccessAction += journalUI.PopulateJournal;
            JournalManager.OnCaptureSuccessAction += journalUI.PopulateJournal;
            LeaderboardsManager.OnFetchSuccessAction += leaderboardsUI.PopulateLeaderboards;
        }

        private void OnDisable()
        {
            AccountManager.OnLoginSuccessAction -= journalManager.InitializeCaptureData;
            JournalManager.OnFetchSuccessAction -= journalUI.PopulateJournal;
            JournalManager.OnCaptureSuccessAction -= journalUI.PopulateJournal;
            LeaderboardsManager.OnFetchSuccessAction -= leaderboardsUI.PopulateLeaderboards;
        }

        public void MainMenuHome()
        {
            accountMenuGroup.SetActive(false);
            mainMenuGroup.SetActive(true);
        }

        public void OnPlayButtonPressed()
        {
            GameManager.Instance.GoToScene("Scanner");
        }

        public void OnLogoutButtonPressed()
        {
            PlayerPrefs.DeleteAll();
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