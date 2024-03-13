using Core;
using DG.Tweening;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Scanner
{
    public class ARPlaceObject : MonoBehaviour
    {
        [SerializeField] private ARRaycastManager raycastManager;
        [SerializeField] private GameObject placementIndicator;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private QuizManager quizManager;

        [SerializeField] private InformationalData placeholderInfo;
        [SerializeField] private DigeomonCaptureData digeomonCaptureData;

        [Header("SFX")]
        [SerializeField] private AudioClip summonSFX;

        private AudioSource audioSource;
        private DigeomonData currDigeomon;
        private ScannerUI scannerUI;
        private GameObject arObject;
        private GameObject spawnedObject;
        private Pose placementPose;
        private bool poseIsValid = false;
        private bool isInitialized = false;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            scannerUI = GetComponent<ScannerUI>();
        }

        private void Start()
        {
            placementIndicator.SetActive(false);
        }

        private void OnEnable()
        {
            JournalManager.OnSummonAction += DirectSetupARObject;
            ScannerUI.OnCaptureAction += quizManager.StartQuiz;
        }

        private void OnDisable()
        {
            JournalManager.OnSummonAction -= DirectSetupARObject;
            ScannerUI.OnCaptureAction -= quizManager.StartQuiz;
        }

        private void Update()
        {
            if (isInitialized)
            {
                if (spawnedObject == null && poseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    PlaceObject();
                }

                UpdatePlacePose();
                UpdatePlacementIndicator();
            }
        }

        public void InitializeARObject(DigeomonData digeomon)
        {
            spawnedObject = null;
            currDigeomon = digeomon;
            arObject = currDigeomon.modelPrefab;
            isInitialized = true;
            scannerUI.ShowSummonHelp();
        }

        private void DirectSetupARObject()
        {
            InitializeARObject(PersistentData.targetDigeomon);
            scannerUI.HideScanner();
        }

        private void UpdatePlacementIndicator()
        {
            if (spawnedObject == null && poseIsValid)
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
            else
            {
                placementIndicator.SetActive(false);
            }
        }

        private void UpdatePlacePose()
        {
            var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            var hits = new List<ARRaycastHit>();
            raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

            poseIsValid = hits.Count > 0;
            if (poseIsValid)
            {
                placementPose = hits[0].pose;
            }
        }

        private void PlaceObject()
        {
            isInitialized = false;
            spawnedObject = Instantiate(arObject, placementPose.position, placementPose.rotation);
            spawnedObject.transform.localScale = Vector3.zero;
            spawnedObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuint);
            audioSource.clip = summonSFX;
            audioSource.Play();

            quizManager.quizUI = spawnedObject.GetComponentInChildren<QuizUI>();

            HologramDisplay hologramDisplay = spawnedObject.GetComponentInChildren<HologramDisplay>();
            hologramDisplay.digeomon = currDigeomon;
            hologramDisplay.SetupEvolveButton();

            HologramCanvas hologramCanvas = spawnedObject.GetComponentInChildren<HologramCanvas>();
            List<InformationalData> infoList = new List<InformationalData>();
            if (currDigeomon.relevantInfos.Count != 0)
                infoList = currDigeomon.relevantInfos;
            else
                infoList.Add(placeholderInfo);
            hologramCanvas.InitializeInformationalData(infoList);

            if (PersistentData.toSummon)
            {
                PersistentData.toSummon = false;
                scannerUI.HideCaptureUI();
                scannerUI.ShowScanner();
                return;
            }

            scannerUI.ShowCaptureButton();
            dialogueManager.StartDialogue(currDigeomon.introDialogue);
        }
    }
}