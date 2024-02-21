using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using Core;
using UI;

namespace Scanner
{
    public class Capture : MonoBehaviour
    {
        [SerializeField] private DigeomonCaptureData digeomonCaptureData;

        private GameManager gameManager;
        private JournalManager journalManager;
        private CaptureUI captureUI;

        private List<DigeomonData> digeomons;
        private ARPlaceObject arPlaceObject;
        private DigeomonData currDigeomon;

        private void Awake()
        {
            arPlaceObject = GetComponent<ARPlaceObject>();

            gameManager = GameManager.Instance;
            journalManager = gameManager.gameObject.GetComponent<JournalManager>();
            captureUI = GetComponent<CaptureUI>();
        }

        private void Start()
        {
            digeomons = gameManager.GetDigeomonList();
        }

        private void OnEnable()
        {
            digeomonCaptureData.OnDigeomonCapture.AddListener(journalManager.AddDigeomon);
            
            CaptureUI.OnSummonAction += SummonDigeomon;
            CaptureUI.OnGoToSceneRequested += gameManager.GoToScene;
        }

        private void OnDisable()
        {
            digeomonCaptureData.OnDigeomonCapture.RemoveListener(journalManager.AddDigeomon);

            CaptureUI.OnSummonAction -= SummonDigeomon;
            CaptureUI.OnGoToSceneRequested -= gameManager.GoToScene;
        }

        public void SearchDigeomon(string label, double acc)
        {
            //double accuracy = Mathf.Round(acc * 100);

            foreach (DigeomonData digeomon in digeomons)
            {
                if (!digeomon.keys.Contains(label))
                    continue;

                if (!digeomonCaptureData.captureData.ContainsKey(digeomon.name))
                {
                    PersistentData.targetDigeomon = digeomon;
                    captureUI.ShowCaptureDialog(digeomon);
                    return;
                }
                else
                {
                    // Tell user that digeomon has been captured already for this particular label
                }
            }

            captureUI.ShowFailDialog();
        }

        private void SummonDigeomon()
        {
            arPlaceObject.InitializeARObject(currDigeomon);
        }

        
    }
}