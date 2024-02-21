using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CaptureUI : MonoBehaviour
    {
        public delegate void SummonDelegate();
        public static event SummonDelegate OnSummonAction;

        public delegate void ScanFailDelegate();
        public static event ScanFailDelegate OnScanFailAction;

        public static event Action<string> OnGoToSceneRequested;

        [Header("Scanner UI")]
        [SerializeField] private GameObject scannerPanel;

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