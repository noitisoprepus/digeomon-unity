using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class HologramDisplay : MonoBehaviour
    {
        public delegate void EvolveDigeomon(GameObject digeomon, DigeomonData evolutionData);
        public static event EvolveDigeomon OnEvolveDigeomonAction;

        [HideInInspector] public DigeomonData digeomon;

        [SerializeField] private DigeomonCaptureData digeomonCaptureData;

        [Header("Hologram Elements")]
        [SerializeField] private GameObject digeomonModel;
        [SerializeField] private GameObject digeomonShape;
        [SerializeField] private GameObject informationPanel;
        [SerializeField] private GameObject evolveButton;

        private HologramCanvas hologramCanvas;
        private bool infoToggle = false;

        private void Awake()
        {
            hologramCanvas = GetComponentInChildren<HologramCanvas>();
        }

        private void Start()
        {
            digeomonShape.SetActive(false);
            informationPanel.transform.localScale = new Vector3(1f, 0f, 1f);
        }

        private void OnEnable()
        {
            QuizUI.OnQuizBeginAction += HideCanvas;
            QuizUI.OnQuizConcludeAction += ShowCanvas;
        }

        private void OnDisable()
        {
            QuizUI.OnQuizBeginAction -= HideCanvas;
            QuizUI.OnQuizConcludeAction -= ShowCanvas;
        }

        public void ShowCanvas()
        {
            SetupEvolveButton();
            hologramCanvas.gameObject.transform.DOScaleY(1f, 0.75f).SetEase(Ease.OutQuart);
        }

        public void HideCanvas()
        {
            hologramCanvas.gameObject.transform.DOScaleY(0f, 0.6f).SetEase(Ease.OutExpo);
        }

        public void ToggleModel()
        {
            if (digeomonModel == null || digeomonShape == null)
                return;

            if (digeomonModel.activeInHierarchy)
            {
                digeomonModel.SetActive(false);
                digeomonShape.SetActive(true);
            }
            else
            {
                digeomonShape.SetActive(false);
                digeomonModel.SetActive(true);
            }
        }

        public void ToggleInformationPanel()
        {
            //informationPanel.SetActive(!informationPanel.activeInHierarchy);
            if (!infoToggle)
            {
                informationPanel.transform.DOScaleY(1f, 0.75f).SetEase(Ease.OutQuart);
                infoToggle = true;
            }
            else
            {
                informationPanel.transform.DOScaleY(0f, 0.6f).SetEase(Ease.OutExpo);
                infoToggle = false;
            }
        }

        public void SetupEvolveButton()
        {
            if (digeomonCaptureData.captureData[digeomon.name] && digeomon.evolution != null)
            {
                if (!digeomonCaptureData.captureData[digeomon.evolution.name])
                    ShowEvolveButton();
                else HideEvolveButton();
            }
            else HideEvolveButton();
        }

        public void OnEvolveButtonPressed()
        {
            PersistentData.targetDigeomon = digeomon.evolution;
            PersistentData.toSummon = false;
            PersistentData.toEvolve = true;

            HideCanvas();
            HideEvolveButton();
            OnEvolveDigeomonAction?.Invoke(digeomonModel, digeomon.evolution);
        }

        private void ShowEvolveButton()
        {
            evolveButton.SetActive(true);
        }

        private void HideEvolveButton()
        {
            evolveButton.SetActive(false);
        }
    }
}