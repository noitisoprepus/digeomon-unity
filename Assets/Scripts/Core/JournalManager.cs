using Core;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class JournalManager : MonoBehaviour
{
    [SerializeField] private GameObject journalPanel;
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
            GameObject entry = Instantiate(entryBox, journalPanel.transform);
            JournalEntryButton entryButton = entry.GetComponent<JournalEntryButton>();
            entryButton.SetPreviewImage(digeomon.Key.modelSprite, digeomon.Value);
        }
    }
}
