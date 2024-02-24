using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class MobilePhoneUI : MonoBehaviour
    {
        [Header("Mobile Phone Views")]
        [SerializeField] private GameObject journalView;
        [SerializeField] private GameObject leaderboardsView;
        [SerializeField] private GameObject settingsView;

        private RectTransform mobilePhone;

        private void Awake()
        {
            mobilePhone = GetComponent<RectTransform>();
        }

        private void Start()
        {
            ResetMobilePhone();
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

        private void ShowMobilePhone()
        {
            mobilePhone.DOAnchorPosY(0f, 1f).SetEase(Ease.OutQuad);
        }

        public void HideMobilePhones()
        {
            mobilePhone.DOAnchorPosY(-3000f, 0.8f).SetEase(Ease.InQuad);
        }

        private void ResetMobilePhone()
        {
            journalView?.SetActive(false);
            leaderboardsView?.SetActive(false);
            settingsView?.SetActive(false);

            mobilePhone.anchoredPosition = new Vector3(0, -3000f, 0f);
        }
    }
}