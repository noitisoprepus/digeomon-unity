using System.Collections.Generic;
using UnityEngine;
using Core;
using UI;

namespace Scanner
{
    public class Capture : MonoBehaviour
    {
        [SerializeField] private DigeomonCaptureData digeomonCaptureData;

        private GameManager gameManager;
        private JournalUI journalUI;
        private ScannerUI scannerUI;
        private List<DigeomonData> digeomons;
        private ARPlaceObject arPlaceObject;
        private DigeomonData currDigeomon;

        private void Awake()
        {
            gameManager = GameManager.Instance;
            arPlaceObject = GetComponent<ARPlaceObject>();
            scannerUI = GetComponent<ScannerUI>();
            journalUI = GetComponent<JournalUI>();
        }

        private void Start()
        {
            digeomons = gameManager.GetDigeomonList();
            journalUI.PopulateJournal();
        }

        private void OnEnable()
        {
            JournalManager.OnCaptureSuccessAction += journalUI.PopulateJournal;
            ScannerUI.OnSummonAction += SummonDigeomon;
            ScannerUI.OnGoToSceneRequested += gameManager.GoToScene;
        }

        private void OnDisable()
        {
            JournalManager.OnCaptureSuccessAction -= journalUI.PopulateJournal;
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
                    PersistentData.targetDigeomon = digeomon;
                    scannerUI.ShowCaptureDialog(digeomon);
                    return;
                }
                else
                {
                    // Tell user that digeomon has been captured already for this particular label
                }
            }

            scannerUI.ShowFailDialog();
        }

        private void SummonDigeomon()
        {
            arPlaceObject.InitializeARObject(currDigeomon);
        }
        
    }
}