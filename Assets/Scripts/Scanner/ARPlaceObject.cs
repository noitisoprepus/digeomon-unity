using Core;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace Scanner
{
    public class ARPlaceObject : MonoBehaviour
    {
        [SerializeField] private ARPlacementInteractable aRPlacementInteractable;
        //[SerializeField] private GameObject placementIndicator;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private VFXManager vfxManager;

        [Header("SFX")]
        [SerializeField] private AudioClip summonSFX;

        private AudioSource audioSource;
        private DigeomonData currDigeomon;
        private ScannerUI scannerUI;
        private GameObject spawnedObject;
        //private Pose placementPose;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            scannerUI = GetComponent<ScannerUI>();
        }

        //private void Start()
        //{
        //    placementIndicator.SetActive(false);
        //}

        private void OnEnable()
        {
            JournalManager.OnSummonAction += DirectSetupARObject;
            aRPlacementInteractable.objectPlaced.AddListener(ObjectPlaced);
        }

        private void OnDisable()
        {
            JournalManager.OnSummonAction -= DirectSetupARObject;
            aRPlacementInteractable.objectPlaced.RemoveAllListeners();
        }

        //private void Update()
        //{
        //    if (isInitialized)
        //    {
        //        if (spawnedObject == null && poseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //        {
        //            PlaceObject();
        //        }

        //        UpdatePlacePose();
        //        UpdatePlacementIndicator();
        //    }
        //}

        public void InitializeARObject(DigeomonData digeomon)
        {
            //isInitialized = true;
            spawnedObject = null;
            currDigeomon = digeomon;
            aRPlacementInteractable.placementPrefab = digeomon.modelPrefab;
            scannerUI.ShowSummonHelp();
        }

        private void DirectSetupARObject()
        {
            InitializeARObject(PersistentData.targetDigeomon);
            scannerUI.HideScanner();
        }

        //private void UpdatePlacementIndicator()
        //{
        //    if (spawnedObject == null && poseIsValid)
        //    {
        //        placementIndicator.SetActive(true);
        //        placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        //    }
        //    else
        //    {
        //        placementIndicator.SetActive(false);
        //    }
        //}

        //private void UpdatePlacePose()
        //{
        //    var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        //    var hits = new List<ARRaycastHit>();
        //    raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        //    poseIsValid = hits.Count > 0;
        //    if (poseIsValid)
        //    {
        //        placementPose = hits[0].pose;
        //    }
        //}

        //private void PlaceObject()
        //{
        //    isInitialized = false;
            
        //    spawnedObject = Instantiate(currDigeomon.modelPrefab, placementPose.position, placementPose.rotation);
        //    spawnedObject.transform.localScale = Vector3.zero;
        //    spawnedObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuint);
            
        //    audioSource.clip = summonSFX;
        //    audioSource.Play();

        //    vfxManager.InitializeDigeomon(spawnedObject, currDigeomon);

        //    if (PersistentData.toSummon)
        //    {
        //        PersistentData.toSummon = false;
        //        scannerUI.HideCaptureUI();
        //        scannerUI.ShowScanner();
        //        return;
        //    }

        //    scannerUI.ShowCaptureButton();
        //    dialogueManager.StartDialogue(currDigeomon.introDialogue);
        //}

        private void ObjectPlaced(ARObjectPlacementEventArgs args)
        {
            aRPlacementInteractable.placementPrefab = null;

            spawnedObject = args.placementObject;
            spawnedObject.transform.localScale = Vector3.zero;
            spawnedObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuint);

            audioSource.clip = summonSFX;
            audioSource.Play();

            vfxManager.InitializeDigeomon(spawnedObject, currDigeomon);

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