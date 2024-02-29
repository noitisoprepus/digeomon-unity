using UnityEngine;
using UnityEngine.Audio;
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

        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        [SerializeField] private AudioMixer musicMixer;
        [SerializeField] private AudioMixer sfxMixer;

        private void OnEnable()
        {
            if (verboseScannerToggle != null && PlayerPrefs.HasKey("verboseScanner"))
                verboseScannerToggle.isOn = PlayerPrefs.GetInt("verboseScanner") == 1;
        }

        public void OnMusicSliderChanged()
        {
            if (musicSlider.value > 0)
            {
                float volume = Mathf.Log10(musicSlider.value) * 20;
                musicMixer.SetFloat("Music_Volume", volume);
            }
            else
            {
                musicMixer.SetFloat("Music_Volume", -80f);
            }
        }

        public void OnSFXSliderChanged()
        {
            if (sfxSlider.value > 0)
            {
                float volume = Mathf.Log10(sfxSlider.value) * 20;
                sfxMixer.SetFloat("SFX_Volume", volume);
            }
            else
            {
                sfxMixer.SetFloat("SFX_Volume", -80f);
            }
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