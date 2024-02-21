using Core;
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

        private DigeomonData currDigeomon;
        private CaptureUI captureUI;
        private GameObject arObject;
        private GameObject spawnedObject;
        private Pose placementPose;
        private bool poseIsValid = false;
        private bool isInitialized = false;

        private void Awake()
        {
            captureUI = GetComponent<CaptureUI>();
        }

        private void Start()
        {
            placementIndicator.SetActive(false);
        }

        public void InitializeARObject(DigeomonData digeomon)
        {
            spawnedObject = null;
            currDigeomon = digeomon;
            arObject = currDigeomon.modelPrefab;
            isInitialized = true;
            captureUI.ShowSummonHelp();
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
            spawnedObject = Instantiate(arObject, placementPose.position, placementPose.rotation);
            // Populate hologram screen
            dialogueManager.StartDialogue(currDigeomon.introDialogue);
            isInitialized = false;
            captureUI.ShowCaptureButton();
        }
    }
}