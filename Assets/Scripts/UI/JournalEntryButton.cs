using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class JournalEntryButton : MonoBehaviour
    {
        public delegate void SummonDigeomon(DigeomonData digeomon);
        public static event SummonDigeomon OnSummonDigeomonAction;

        public delegate void EvolveDigeomon(DigeomonData digeomon);
        public static event EvolveDigeomon OnEvolveDigeomonAction;

        public delegate void ShowInfo(DigeomonType infoType);
        public static event ShowInfo OnShowInfoAction;

        [SerializeField] private Image previewImage;
        [SerializeField] private GameObject checkmarkObj;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [Header("Bottom Bar Buttons")]
        [SerializeField] private Button summonButton;
        [SerializeField] private Button evolveButton;
        [SerializeField] private Button infoButton;

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

            summonButton.interactable = isCaught;
            evolveButton.interactable = isCaught;
            infoButton.interactable = isCaught;
        }

        public void UpdateJournalEntry(Dictionary<string, bool> captureData)
        {
            UpdateJournalEntry(captureData[digeomon.name]);
        }

        public void OnSummonButtonPressed()
        {
            OnSummonDigeomonAction?.Invoke(digeomon);
        }

        public void OnEvolveButtonPressed()
        {
            OnEvolveDigeomonAction?.Invoke(digeomon.evolution);
        }

        public void OnShowInfoButtonPressed()
        {
            // Add ShowInfo function in JournalUI
            OnShowInfoAction?.Invoke(digeomon.type);
        }
    }
}