using System.Collections;
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
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private VFXManager vfxManager;

        [Header("SFX")]
        [SerializeField] private AudioClip summonSFX;

        private AudioSource audioSource;
        private DigeomonData currDigeomon;
        private ScannerUI scannerUI;
        private GameObject spawnedObject;
        private bool isSpawned = true;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            scannerUI = GetComponent<ScannerUI>();
        }

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

        public void InitializeARObject(DigeomonData digeomon)
        {
            spawnedObject = null;
            currDigeomon = digeomon;
            scannerUI.ShowSummonHelp();
            StartCoroutine(StartPlacementDelay());
        }

        private void DirectSetupARObject()
        {
            InitializeARObject(PersistentData.targetDigeomon);
            scannerUI.HideScanner();
        }

        private IEnumerator StartPlacementDelay()
        {
            yield return new WaitForSeconds(1f);
            aRPlacementInteractable.placementPrefab = currDigeomon.modelPrefab;
            isSpawned = false;
        }

        private void ObjectPlaced(ARObjectPlacementEventArgs args)
        {
            if (isSpawned)
            {
                Destroy(args.placementObject);
                return;
            }
            isSpawned = true;
            //aRPlacementInteractable.placementPrefab = null;

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