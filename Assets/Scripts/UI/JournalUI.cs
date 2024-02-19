using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class JournalUI : MonoBehaviour
    {
        public delegate void JournalDelegate();
        public static event JournalDelegate OnJournalAction;

        [SerializeField] private GameObject journalContent;
        [SerializeField] private GameObject entryBox;
        
        private void OnEnable()
        {
            OnJournalAction?.Invoke();
        }

        private void PopulateJournal(Dictionary<DigeomonData, bool> captureData)
        {
            foreach (KeyValuePair<DigeomonData, bool> digeomon in captureData)
            {
                GameObject entry = Instantiate(entryBox, journalContent.transform);
                JournalEntryButton entryButton = entry.GetComponent<JournalEntryButton>();
                entryButton.SetPreviewImage(digeomon.Key.modelSprite, digeomon.Value);
            }
        }
    }
}