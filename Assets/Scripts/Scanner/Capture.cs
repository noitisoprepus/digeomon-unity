using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.XR.CoreUtils;
using Core;

namespace Scanner
{
    public class Capture : MonoBehaviour
    {
        [Header("Capture GUI")]
        public XROrigin xrOrigin;
        public GameObject scannerPanel;
        public GameObject successPanel;
        public GameObject captureDialog;
        public GameObject failDialog;
        public Image silhouette;

        private GameManager gameManager;

        private List<DigeomonData> digeomons;
        private ARPlaceObject arPlaceObject;
        private RectTransform failDialogRT;
        private DigeomonData currDigeomon;

        private void Awake()
        {
            arPlaceObject = xrOrigin.gameObject.GetComponent<ARPlaceObject>();
            failDialogRT = failDialog.GetComponent<RectTransform>();
        }

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;

            successPanel.SetActive(false);
            failDialog.SetActive(false);
            digeomons = gameManager.GetDigeomonList();
        }

        public void SearchDigeomon(string label, double acc)
        {
            //double accuracy = Mathf.Round(acc * 100);

            foreach (DigeomonData digeomon in digeomons)
            {
                if (!digeomon.keys.Contains(label))
                    continue;

                if (!gameManager.GetDigeomonCaptures().Contains(digeomon.name))
                {
                    ShowCaptureDialog(digeomon);
                    return;
                }
                else
                {
                    // Tell user that digeomon has been captured already for this particular label
                }
            }

            ShowFailDialog();
        }

        private void ShowCaptureDialog(DigeomonData digeomon)
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
            arPlaceObject.InitializeARObject(currDigeomon);
            scannerPanel.SetActive(false);
            OnCloseButtonPressed();
        }

        public void OnCloseButtonPressed()
        {
            successPanel.SetActive(false);
        }

        public void OnCaptureButtonPressed()
        {
            PersistentData.targetDigeomon = currDigeomon;
            // gameManager.GoToScene("Sandbox");
            gameManager.CaptureDigeomon(currDigeomon);
            OnCloseButtonPressed();
        }
    }
}