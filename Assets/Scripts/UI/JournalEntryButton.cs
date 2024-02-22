using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class JournalEntryButton : MonoBehaviour
    {
        [SerializeField] private Image previewImage;
        [SerializeField] private GameObject checkmarkObj;
        [SerializeField] private TextMeshProUGUI nameText;

        private DigeomonData digeomon;

        public void InitializeJournalEntry(DigeomonData data, bool isCaught)
        {
            digeomon = data;
            previewImage.sprite = digeomon.modelSprite;
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
    }
}