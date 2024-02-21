using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScannerUI : MonoBehaviour
    {
        public delegate void ScanDelegate();
        public static event ScanDelegate OnScanAction;

        public delegate void SummonDelegate();
        public static event SummonDelegate OnSummonAction;

        public static event Action<string> OnGoToSceneRequested;

        [Header("Scanner UI")]
        [SerializeField] private GameObject scannerPanel;
        [SerializeField] private Button scanButton;
        [SerializeField] private List<Image> scanFrameSprites;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color disabledColor;
        [SerializeField] private TextMeshProUGUI detectedObjText;

        [Header("Capture UI")]
        [SerializeField] private GameObject successPanel;
        [SerializeField] private GameObject captureDialog;
        [SerializeField] private GameObject failDialog;
        [SerializeField] private Image silhouette;

        [Header("Summon UI")]
        [SerializeField] private GameObject helpPanel;
        [SerializeField] private GameObject captureButton;

        private RectTransform failDialogRT;

        private void Awake()
        {
            failDialogRT = failDialog.GetComponent<RectTransform>();
        }

        private void Start()
        {
            successPanel.SetActive(false);
            failDialog.SetActive(false);
            helpPanel.SetActive(false);
            captureButton.SetActive(false);
        }

        public void OnScanButtonPressed()
        {
            scanButton.interactable = false;
            scanButton.GetComponentInChildren<TextMeshProUGUI>().text = ". . .";
            foreach (Image img in scanFrameSprites)
                img.color = disabledColor;
            OnScanAction?.Invoke();
        }

        public void OnScanFinished()
        {
            scanButton.interactable = true;
            scanButton.GetComponentInChildren<TextMeshProUGUI>().text = "SCAN";
            foreach (Image img in scanFrameSprites)
                img.color = normalColor;
        }

        public void ShowCaptureDialog(DigeomonData digeomon)
        {
            successPanel.SetActive(true);
            silhouette.sprite = digeomon.modelSprite;
            captureDialog.transform.localScale = Vector3.zero;
            captureDialog.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutCubic);
        }

        public void ShowFailDialog()
        {
            failDialogRT.anchoredPosition = new Vector2(300f, failDialogRT.anchoredPosition.y);
            failDialog.SetActive(true);
            failDialogRT.DOAnchorPosX(-300f, 1.25f).SetEase(Ease.OutQuad);
            failDialogRT.DOAnchorPosX(300f, 0.75f).SetEase(Ease.InQuad).SetDelay(3f);
        }

        public void OnSummonButtonPressed()
        {
            OnSummonAction?.Invoke();
            scannerPanel.SetActive(false);
            OnCloseButtonPressed();
        }

        public void OnCloseButtonPressed()
        {
            successPanel.SetActive(false);
        }

        public void OnCaptureButtonPressed()
        {
            OnGoToSceneRequested?.Invoke("Sandbox");
        }

        public void ShowSummonHelp()
        {
            helpPanel.SetActive(true);
            captureButton.SetActive(false);
        }

        public void ShowCaptureButton()
        {
            captureButton.SetActive(true);
            helpPanel.SetActive(false);
        }
    }
}