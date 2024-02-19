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
        [SerializeField] private XROrigin xrOrigin;
        [SerializeField] private GameObject scannerPanel;
        [SerializeField] private GameObject successPanel;
        [SerializeField] private GameObject captureDialog;
        [SerializeField] private GameObject failDialog;
        [SerializeField] private Image silhouette;

        [Header("Capture Data")]
        [SerializeField] private DigeomonCaptureData digeomonCaptureData;

        private GameManager gameManager;
        private JournalManager journalManager;

        private List<DigeomonData> digeomons;
        private ARPlaceObject arPlaceObject;
        private RectTransform failDialogRT;
        private DigeomonData currDigeomon;

        private void Awake()
        {
            arPlaceObject = xrOrigin.gameObject.GetComponent<ARPlaceObject>();
            failDialogRT = failDialog.GetComponent<RectTransform>();

            gameManager = GameManager.Instance;
            journalManager = gameManager.gameObject.GetComponent<JournalManager>();
        }

        private void Start()
        {
            successPanel.SetActive(false);
            failDialog.SetActive(false);
            digeomons = gameManager.GetDigeomonList();
        }

        private void OnEnable()
        {
            digeomonCaptureData.OnDigeomonCapture.AddListener(journalManager.AddDigeomon);
        }

        private void OnDisable()
        {
            digeomonCaptureData.OnDigeomonCapture.RemoveListener(journalManager.AddDigeomon);
        }

        public void SearchDigeomon(string label, double acc)
        {
            //double accuracy = Mathf.Round(acc * 100);

            foreach (DigeomonData digeomon in digeomons)
            {
                if (!digeomon.keys.Contains(label))
                    continue;

                if (!digeomonCaptureData.captureData.ContainsKey(digeomon.name))
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
            // Direct catch
            digeomonCaptureData.CaptureDigeomon(currDigeomon);
            OnCloseButtonPressed();
        }
    }
}