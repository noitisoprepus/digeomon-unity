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
                    DigeomonData data = digeomon.Value;

                    if (data.preEvolution != null && !digeomonCaptureData.captureData[data.preEvolution.name])
                        continue;

                    GameObject entry = Instantiate(entryBox, journalContent.transform);
                    JournalEntryButton entryButton = entry.GetComponent<JournalEntryButton>();
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

        public void ResetJournal()
        {
            foreach (Transform entry in journalContent.transform)
                Destroy(entry.gameObject);
            journalContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}