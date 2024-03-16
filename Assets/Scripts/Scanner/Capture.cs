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
        private VFXManager vfxManager;
        private ARPlaceObject arPlaceObject;
        private JournalManager journalManager;
        private JournalUI journalUI;
        private ScannerUI scannerUI;
        private List<DigeomonData> digeomons;
        private DigeomonData currDigeomon;

        private void Awake()
        {
            gameManager = GameManager.Instance;
            vfxManager= GetComponent<VFXManager>();
            arPlaceObject = GetComponent<ARPlaceObject>();
            journalManager = gameManager.gameObject.GetComponent<JournalManager>();
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

            HologramDisplay.OnEvolveDigeomonAction += vfxManager.InitializeEvolution;

            ScannerUI.OnSummonAction += SummonDigeomon;
            ScannerUI.OnGoToSceneRequested += gameManager.GoToScene;
        }

        private void OnDisable()
        {
            JournalManager.OnCaptureSuccessAction -= journalUI.PopulateJournal;
            JournalManager.OnSummonAction -= mobilePhoneUI.HideMobilePhones;

            JournalEntryButton.OnSummonDigeomonAction -= journalManager.SummonDigeomon;

            HologramDisplay.OnEvolveDigeomonAction -= vfxManager.InitializeEvolution;

            ScannerUI.OnSummonAction -= SummonDigeomon;
            ScannerUI.OnGoToSceneRequested -= gameManager.GoToScene;
        }

        public void SearchDigeomon(string label)
        {
            string labelLowerCase = label.ToLower();
            foreach (DigeomonData digeomon in digeomons)
            {
                foreach(string key in digeomon.keys)
                {
                    if (labelLowerCase.Contains(key.ToLower()))
                    {
                        TriggerCapture(digeomon);
                        scannerUI.OnScanFinished();
                        return;
                    }
                }
            }
            scannerUI.OnScanFinished();
            scannerUI.ShowFailDialog();
        }

        private void TriggerCapture(DigeomonData digeomon)
        {
            if (!digeomonCaptureData.captureData[digeomon.name])
            {
                currDigeomon = digeomon;
                PersistentData.targetDigeomon = digeomon;
                scannerUI.ShowCaptureDialog(digeomon);
            }
            else
            {
                gameManager.ShowDialog(digeomon.name + " has \nalready been found");
            }
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