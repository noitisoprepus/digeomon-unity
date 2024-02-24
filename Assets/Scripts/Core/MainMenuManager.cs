using DG.Tweening;
using Firebase.Auth;
using UI;
using UnityEngine;

namespace Core
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuGroup;
        [SerializeField] private GameObject accountMenuGroup;

        [Header("Mobile Phone GUI")]
        [SerializeField] private RectTransform mobilePhone;

        [Header("Mobile Phone Views")]
        [SerializeField] private GameObject journalView;
        [SerializeField] private GameObject leaderboardsView;
        [SerializeField] private GameObject settingsView;

        private GameManager gameManager;
        private JournalManager journalManager;
        private FirebaseAuth auth;
        
        private JournalUI journalUI;
        private LeaderboardsUI leaderboardsUI;

        private void Awake()
        {
            gameManager = GameManager.Instance;
            journalManager = gameManager.gameObject.GetComponent<JournalManager>();
            auth = FirebaseAuth.DefaultInstance;

            journalUI = GetComponent<JournalUI>();
            leaderboardsUI = GetComponent<LeaderboardsUI>();

            if (auth.CurrentUser == null)
                ShowAccountMenu();
            else
                gameManager.userID = auth.CurrentUser.UserId;
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

        private void Start()
        {
            ResetMobilePhone();
            journalUI.PopulateJournal();
        }

        public void MainMenuHome()
        {
            ResetMobilePhone();
            accountMenuGroup.SetActive(false);
            mainMenuGroup.SetActive(true);
        }

        public void OnPlayButtonPressed()
        {
            gameManager.GoToScene("Scanner");
        }

        public void OnJournalButtonPressed()
        {
            ResetMobilePhone();
            journalView.SetActive(true);
            ShowMobilePhone();
        }

        public void OnLeaderboardsButtonPressed()
        {
            ResetMobilePhone();
            leaderboardsView.SetActive(true);
            ShowMobilePhone();
        }

        public void OnSettingsButtonPressed()
        {
            ResetMobilePhone();
            settingsView.SetActive(true);
            ShowMobilePhone();
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

        private void ShowMobilePhone()
        {
            mobilePhone.DOAnchorPosY(960f, 1f).SetEase(Ease.OutQuad);
        }

        public void HideMobilePhones()
        {
            mobilePhone.DOAnchorPosY(-960f, 0.6f).SetEase(Ease.InQuad);
        }

        private void ResetMobilePhone()
        {
            journalView.SetActive(false);
            leaderboardsView.SetActive(false);
            settingsView.SetActive(false);

            mobilePhone.anchoredPosition = new Vector3(0, -960f, 0f);
        }
    }
}