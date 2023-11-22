using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;
using DG.Tweening;

public class Capture : MonoBehaviour
{
    [Header("Capture GUI")]
    public ARPlacementInteractable placementInteractable;
    public GameObject successPanel;
    public GameObject captureDialog;
    public GameObject failDialog;
    public Image silhouette;

    [Header("Digeomon Data")]
    public List<Digeomon> digeomons;

    private RectTransform failDialogRT;

    private void Awake()
    {
        failDialogRT = failDialog.GetComponent<RectTransform>();
    }

    private void Start()
    {
        successPanel.SetActive(false);
        failDialog.SetActive(false);
    }

    public void SearchDigeomon(string label, double acc)
    {
        //double accuracy = Mathf.Round(acc * 100);

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

        ShowFailDialog();
    }

    private void ShowCaptureDialog(Digeomon digeomon)
    {
        successPanel.SetActive(true);
        silhouette.sprite = digeomon.modelSprite;
        captureDialog.transform.localScale = Vector3.zero;
        captureDialog.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutCubic);
        placementInteractable.placementPrefab = digeomon.modelPrefab;
    }

    private void ShowFailDialog()
    {
        failDialogRT.anchoredPosition = new Vector2(300f, failDialogRT.anchoredPosition.y);
        failDialog.SetActive(true);
        failDialogRT.DOAnchorPosX(-300f, 1.5f).SetEase(Ease.OutQuad);
        failDialogRT.DOAnchorPosX(300f, 1.5f).SetEase(Ease.InQuad).SetDelay(3f);
    }

    public void OnSummonButtonPressed()
    {
        OnCloseButtonPressed();
    }

    public void OnCloseButtonPressed()
    {
        successPanel.SetActive(false);
    }
}
