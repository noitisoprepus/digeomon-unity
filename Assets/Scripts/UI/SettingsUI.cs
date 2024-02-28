using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsUI : MonoBehaviour
    {
        public delegate void LogoutPlayer();
        public static event LogoutPlayer OnLogoutPlayerAction;

        [SerializeField] private DigeomonCaptureData digeomonCaptureData;

        [SerializeField] private MobilePhoneUI mobilePhoneUI;
        [SerializeField] private Toggle verboseScannerToggle;

        // Add music slider
        // Add sfx slider

        private void OnEnable()
        {
            if (verboseScannerToggle != null && PlayerPrefs.HasKey("verboseScanner"))
                verboseScannerToggle.isOn = PlayerPrefs.GetInt("verboseScanner") == 1;
        }

        public void OnVerboseScannerToggleValueChanged()
        {
            PlayerPrefs.SetInt("verboseScanner", verboseScannerToggle.isOn ? 1 : 0);
        }

        public void OnLogoutButtonPressed()
        {
            digeomonCaptureData.ResetCaptureData();
            OnLogoutPlayerAction?.Invoke();
            mobilePhoneUI.HideMobilePhones();
        }

        public void OnResetPlayerPrefsButtonPressed()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}