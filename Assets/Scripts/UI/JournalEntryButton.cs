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

        [SerializeField] private Image previewImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [Header("Bottom Bar Buttons")]
        [SerializeField] private Button summonButton;

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
            nameText.text = (isCaught) ? digeomon.name : "???";

            summonButton.interactable = isCaught;
        }

        public void UpdateJournalEntry(Dictionary<string, bool> captureData)
        {
            UpdateJournalEntry(captureData[digeomon.name]);
        }

        public void OnSummonButtonPressed()
        {
            OnSummonDigeomonAction?.Invoke(digeomon);
        }
    }
}