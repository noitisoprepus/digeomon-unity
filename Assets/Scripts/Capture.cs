using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.XR.CoreUtils;

public class Capture : MonoBehaviour
{
    [Header("Capture GUI")]
    public XROrigin xrOrigin;
    public GameObject successPanel;
    public GameObject captureDialog;
    public GameObject failDialog;
    public Image silhouette;

    [Header("Digeomon Data")]
    public List<Digeomon> digeomons;

    private ARPlaceObject arPlaceObject;
    private RectTransform failDialogRT;
    private Digeomon currDigeomon;

    private void Awake()
    {
        arPlaceObject = xrOrigin.gameObject.GetComponent<ARPlaceObject>();
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
        currDigeomon = digeomon;
        successPanel.SetActive(true);
        silhouette.sprite = currDigeomon.modelSprite;
        captureDialog.transform.localScale = Vector3.zero;
        captureDialog.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutCubic);
    }

    private void ShowFailDialog()
    {
        failDialogRT.anchoredPosition = new Vector2(300f, failDialogRT.anchoredPosition.y);
        failDialog.SetActive(true);
        failDialogRT.DOAnchorPosX(-300f, 1.25f).SetEase(Ease.OutQuad);
        failDialogRT.DOAnchorPosX(300f, 0.75f).SetEase(Ease.InQuad).SetDelay(3f);
    }

    public void OnSummonButtonPressed()
    {
        arPlaceObject.InitializeARObject(currDigeomon.modelPrefab);
        OnCloseButtonPressed();
    }

    public void OnCloseButtonPressed()
    {
        successPanel.SetActive(false);
    }
}
