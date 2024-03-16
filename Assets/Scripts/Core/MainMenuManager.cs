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
        private LeaderboardsManager leaderboardsManager;

        private FirebaseAuth auth;
        
        private JournalUI journalUI;
        private LeaderboardsUI leaderboardsUI;

        private void Awake()
        {
            journalManager = GameManager.Instance.gameObject.GetComponent<JournalManager>();
            leaderboardsManager = GetComponent<LeaderboardsManager>();

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
                leaderboardsManager.InitializeLeaderboardsData();
                MainMenuHome();
            }
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt("guest") == 1)
            {
                journalManager.InitializeCaptureData();
                leaderboardsManager.InitializeLeaderboardsData();
                MainMenuHome();
            }
        }

        private void OnEnable()
        {
            AccountManager.OnLoginSuccessAction += journalManager.InitializeCaptureData;
            AccountManager.OnLoginSuccessAction += leaderboardsManager.InitializeLeaderboardsData;

            JournalManager.OnFetchSuccessAction += journalUI.PopulateJournal;
            JournalManager.OnCaptureSuccessAction += journalUI.PopulateJournal;

            LeaderboardsManager.OnFetchSuccessAction += leaderboardsUI.PopulateLeaderboards;
            LeaderboardsManager.OnFetchCancelAction += leaderboardsUI.ShowGuestWarning;

            JournalEntryButton.OnSummonDigeomonAction += journalManager.SummonDigeomon;

            SettingsUI.OnLogoutPlayerAction += LogoutPlayer;
            SettingsUI.OnLogoutPlayerAction += journalUI.ResetJournal;
            SettingsUI.OnLogoutPlayerAction += leaderboardsUI.ResetLeaderboards;
        }

        private void OnDisable()
        {
            AccountManager.OnLoginSuccessAction -= journalManager.InitializeCaptureData;
            AccountManager.OnLoginSuccessAction -= leaderboardsManager.InitializeLeaderboardsData;

            JournalManager.OnFetchSuccessAction -= journalUI.PopulateJournal;
            JournalManager.OnCaptureSuccessAction -= journalUI.PopulateJournal;

            LeaderboardsManager.OnFetchSuccessAction -= leaderboardsUI.PopulateLeaderboards;
            LeaderboardsManager.OnFetchCancelAction -= leaderboardsUI.ShowGuestWarning;

            JournalEntryButton.OnSummonDigeomonAction -= journalManager.SummonDigeomon;

            SettingsUI.OnLogoutPlayerAction -= LogoutPlayer;
            SettingsUI.OnLogoutPlayerAction -= journalUI.ResetJournal;
            SettingsUI.OnLogoutPlayerAction -= leaderboardsUI.ResetLeaderboards;
        }

        public void MainMenuHome()
        {
            accountMenuGroup.SetActive(false);
            mainMenuGroup.SetActive(true);
        }

        public void OnPlayButtonPressed()
        {
            PersistentData.toSummon = false;
            GameManager.Instance.GoToScene("Scanner");
        }

        private void LogoutPlayer()
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