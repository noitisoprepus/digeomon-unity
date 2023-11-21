using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class Capture : MonoBehaviour
{
    [Header("Capture GUI")]
    public ARPlacementInteractable placementInteractable;
    public GameObject successPanel;
    public GameObject failPanel;
    public Image silhouette;

    [Header("Scanner GUI")]
    public Button scanButton;

    [Header("Digeomon Data")]
    public List<Digeomon> digeomons;

    private void Start()
    {
        successPanel.SetActive(false);
        failPanel.SetActive(false);
    }

    public void SearchDigeomon(string label, double acc)
    {
        //double accuracy = Math.Round(acc * 100, 1);

        foreach (Digeomon digeomon in digeomons)
        {
            foreach (string key in digeomon.keys)
            {
                if (label.Contains(key))
                {
                    ShowCaptureDialog(digeomon);
                    return;
                }
            }
        }

        // Make into a pop-up notification
        failPanel.SetActive(true);
    }

    private void ShowCaptureDialog(Digeomon digeomon)
    {
        silhouette.sprite = digeomon.modelSprite;
        successPanel.SetActive(true);
        placementInteractable.placementPrefab = digeomon.modelPrefab;
    }

    public void OnSummonButtonPressed()
    {
        successPanel.SetActive(false);
        failPanel.SetActive(false);
    }
}
