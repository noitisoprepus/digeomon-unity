using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class JournalUI : MonoBehaviour
    {
        [Header("Capture Data")]
        [SerializeField] private DigeomonCaptureData digeomonCaptureData;

        [Header("GUI")]
        [SerializeField] private GameObject journalContent;
        [SerializeField] private GameObject entryBox;

        private void OnEnable()
        {
            UpdateJournal();
        }

        public void PopulateJournal()
        {
            if (journalContent.transform.childCount == 0)
            {
                foreach (KeyValuePair<string, DigeomonData> digeomon in digeomonCaptureData.digeomonData)
                {
                    GameObject entry = Instantiate(entryBox, journalContent.transform);
                    JournalEntryButton entryButton = entry.GetComponent<JournalEntryButton>();
                    DigeomonData data = digeomon.Value;
                    entryButton.InitializeJournalEntry(data, digeomonCaptureData.captureData[data.name]);
                }
            }
            else
            {
                UpdateJournal();
            }
        }

        private void UpdateJournal()
        {
            foreach (Transform entry in journalContent.transform)
            {
                entry.GetComponent<JournalEntryButton>().UpdateJournalEntry(digeomonCaptureData.captureData);
            }
        }
    }
}