using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class JournalEntryButton : MonoBehaviour
    {
        public delegate void ShowInfo(DigeomonType infoType);
        public static event ShowInfo OnShowInfoAction;

        [SerializeField] private Image previewImage;
        [SerializeField] private GameObject checkmarkObj;
        [SerializeField] private GameObject evolveButton;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        private DigeomonData digeomon;

        public void InitializeJournalEntry(DigeomonData data, bool isCaught)
        {
            digeomon = data;
            previewImage.sprite = digeomon.modelSprite;
            descriptionText.text = digeomon.description;
            UpdateJournalEntry(isCaught);
        }

        public void UpdateJournalEntry(bool isCaught)
        {
            previewImage.color = isCaught ? Color.white : Color.black;
            checkmarkObj.SetActive(!isCaught);
            nameText.text = (isCaught) ? digeomon.name : "???";
        }

        public void UpdateJournalEntry(Dictionary<string, bool> captureData)
        {
            UpdateJournalEntry(captureData[digeomon.name]);
        }

        public void OnSummonButtonPressed()
        {
            // If current scene is MainMenu, Go to scanner
            // Else, proceed to summoning directly
        }

        public void OnEvolveButtonPressed()
        {
            // Target digeomon = digeomon.evolution
            // Go to sandbox
        }

        public void OnShowInfoButtonPressed()
        {
            // Add ShowInfo function in JournalUI
            OnShowInfoAction?.Invoke(digeomon.type);
        }
    }
}