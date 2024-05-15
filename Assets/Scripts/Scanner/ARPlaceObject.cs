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
        private bool toSpawn = false;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            scannerUI = GetComponent<ScannerUI>();
        }

        private void OnEnable()
        {
            JournalManager.OnSummonAction += DirectSetupARObject;

            DialogueManager.OnDialogueStartAction += DisablePlacement;
            DialogueManager.OnDialogueEndAction += EnablePlacement;
            MobilePhoneUI.OnMobilePhoneShowAction += DisablePlacement;
            MobilePhoneUI.OnMobilePhoneHideAction += EnablePlacement;
            ScannerUI.OnSuccessShowAction += DisablePlacement;
            ScannerUI.OnSuccessHideAction += EnablePlacement;

            aRPlacementInteractable.objectPlaced.AddListener(ObjectPlaced);
        }

        private void OnDisable()
        {
            JournalManager.OnSummonAction -= DirectSetupARObject;

            DialogueManager.OnDialogueStartAction -= DisablePlacement;
            DialogueManager.OnDialogueEndAction -= EnablePlacement;
            MobilePhoneUI.OnMobilePhoneShowAction -= DisablePlacement;
            MobilePhoneUI.OnMobilePhoneHideAction -= EnablePlacement;
            ScannerUI.OnSuccessShowAction -= DisablePlacement;
            ScannerUI.OnSuccessHideAction -= EnablePlacement;

            aRPlacementInteractable.objectPlaced.RemoveAllListeners();
        }

        public void InitializeARObject(DigeomonData digeomon)
        {
            scannerUI.ShowSummonHelp();
            spawnedObject = null;
            currDigeomon = digeomon;
            aRPlacementInteractable.placementPrefab = currDigeomon.modelPrefab;
            toSpawn = true;
        }

        private void DirectSetupARObject()
        {
            InitializeARObject(PersistentData.targetDigeomon);
            scannerUI.HideScanner();
        }

        private void ObjectPlaced(ARObjectPlacementEventArgs args)
        {
            if (isSpawned || !toSpawn)
            {
                Destroy(args.placementObject);
                return;
            }
            isSpawned = true;
            toSpawn = false;

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

        private void DisablePlacement()
        {
            StopAllCoroutines();
            isSpawned = true;
        }

        private void EnablePlacement()
        {
            StartCoroutine(EnablePlacementDelayedEnum());
        }

        private IEnumerator EnablePlacementDelayedEnum()
        {
            yield return new WaitForSeconds(1.5f);
            isSpawned = false;
        }
    }
}