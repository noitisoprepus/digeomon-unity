using System.Collections.Generic;
using UnityEngine;
using Core;
using UI;

namespace Scanner
{
    public class Capture : MonoBehaviour
    {
        [SerializeField] private DigeomonCaptureData digeomonCaptureData;
        
        [SerializeField] private MobilePhoneUI mobilePhoneUI;

        private GameManager gameManager;
        private JournalManager journalManager;
        private JournalUI journalUI;
        private ScannerUI scannerUI;
        private List<DigeomonData> digeomons;
        private ARPlaceObject arPlaceObject;
        private DigeomonData currDigeomon;

        private void Awake()
        {
            gameManager = GameManager.Instance;
            journalManager = gameManager.gameObject.GetComponent<JournalManager>();
            arPlaceObject = GetComponent<ARPlaceObject>();
            scannerUI = GetComponent<ScannerUI>();
            journalUI = GetComponent<JournalUI>();
        }

        private void Start()
        {
            digeomons = gameManager.GetDigeomonList();
            journalUI.PopulateJournal();

            if (PersistentData.toSummon)
            {
                scannerUI.HideScanner();
                InitializeDigeomonSummon(PersistentData.targetDigeomon);
            }
        }

        private void OnEnable()
        {
            JournalManager.OnCaptureSuccessAction += journalUI.PopulateJournal;
            JournalManager.OnSummonAction += mobilePhoneUI.HideMobilePhones;

            JournalEntryButton.OnSummonDigeomonAction += journalManager.SummonDigeomon;
            JournalEntryButton.OnEvolveDigeomonAction += journalManager.EvolveDigeomon;

            ScannerUI.OnSummonAction += SummonDigeomon;
            ScannerUI.OnGoToSceneRequested += gameManager.GoToScene;
        }

        private void OnDisable()
        {
            JournalManager.OnCaptureSuccessAction -= journalUI.PopulateJournal;
            JournalManager.OnSummonAction -= mobilePhoneUI.HideMobilePhones;

            JournalEntryButton.OnSummonDigeomonAction -= journalManager.SummonDigeomon;
            JournalEntryButton.OnEvolveDigeomonAction -= journalManager.EvolveDigeomon;

            ScannerUI.OnSummonAction -= SummonDigeomon;
            ScannerUI.OnGoToSceneRequested -= gameManager.GoToScene;
        }

        public void SearchDigeomon(string label)
        {
            foreach (DigeomonData digeomon in digeomons)
            {
                if (!digeomon.keys.Contains(label))
                    continue;

                if (!digeomonCaptureData.captureData.ContainsKey(digeomon.name))
                {
                    currDigeomon = digeomon;
                    PersistentData.targetDigeomon = digeomon;
                    scannerUI.ShowCaptureDialog(digeomon);
                    return;
                }
                else
                {
                    gameManager.ShowDialog(digeomon.name + "has \nalready been found");
                }
            }

            scannerUI.ShowFailDialog();
        }

        private void SummonDigeomon()
        {
            InitializeDigeomonSummon(currDigeomon);
        }

        private void InitializeDigeomonSummon(DigeomonData digeomonData)
        {
            arPlaceObject.InitializeARObject(digeomonData);
        }
        
    }
}