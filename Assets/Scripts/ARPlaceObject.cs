using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;

    [Header("Capture UI")]
    public GameObject helpPanel;
    public GameObject captureButton;

    private GameObject arObject;
    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool poseIsValid = false;
    private bool isInitialized = false;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private void Start()
    {
        placementIndicator.SetActive(false);
        helpPanel.SetActive(false);
        captureButton.SetActive(false);
    }

    public void InitializeARObject(GameObject objPrefab)
    {
        spawnedObject = null;
        arObject = objPrefab;
        isInitialized = true;
        helpPanel.SetActive(true);
        captureButton.SetActive(false);
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
        isInitialized = false;
        helpPanel.SetActive(false);
        captureButton.SetActive(true);
    }
}
