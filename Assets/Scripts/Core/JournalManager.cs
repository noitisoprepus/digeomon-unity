using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Core
{
    public class JournalManager : MonoBehaviour
    {
        // TODO: Move UI stuff to separate script. This will now be a Global script which will remember the captured digeomons.
        [SerializeField] private GameObject journalContent;
        [SerializeField] private GameObject entryBox;

        private Dictionary<DigeomonData, bool> caughtDigeomons;

        private void Start()
        {
            GetCaughtDigeomonData();
        }

        private void GetCaughtDigeomonData()
        {
            caughtDigeomons = new Dictionary<DigeomonData, bool>();
            List<DigeomonData> availableDigeomons = GameManager.Instance.GetDigeomonList();
            foreach (DigeomonData digeomon in availableDigeomons)
            {
                caughtDigeomons.Add(digeomon, false);
                // Sync value with database (use digeomon.name as reference key)
            }
            // Populate Journal once data is finished syncing
            PopulateJournal();
        }

        private void PopulateJournal()
        {
            foreach (KeyValuePair<DigeomonData, bool> digeomon in caughtDigeomons)
            {
                GameObject entry = Instantiate(entryBox, journalContent.transform);
                JournalEntryButton entryButton = entry.GetComponent<JournalEntryButton>();
                entryButton.SetPreviewImage(digeomon.Key.modelSprite, digeomon.Value);
            }
        }
    }
}