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
        
        public void PopulateJournal()
        {
            foreach (KeyValuePair<string, DigeomonData> digeomon in digeomonCaptureData.digeomonData)
            {
                GameObject entry = Instantiate(entryBox, journalContent.transform);
                JournalEntryButton entryButton = entry.GetComponent<JournalEntryButton>();
                DigeomonData data = digeomon.Value;
                entryButton.InitializeJournalEntry(data, digeomonCaptureData.captureData[data.name]);
            }
        }

        public void UpdateJournal()
        {
            foreach (Transform entry in transform)
            {
                // TODO: Repopulation/Updating
                //entry.GetComponent<JournalEntryButton>().UpdateJournalEntry();
            }
        }
    }
}