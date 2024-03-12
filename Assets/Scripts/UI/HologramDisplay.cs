using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class HologramDisplay : MonoBehaviour
    {
        public delegate void EvolveDigeomon(GameObject digeomon, DigeomonData evolutionData);
        public static event EvolveDigeomon OnEvolveDigeomonAction;

        [HideInInspector] public DigeomonData digeomon;

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
            if (SceneManager.GetActiveScene().name.Equals("Sandbox"))
            {
                gameObject.SetActive(false);
                return;
            }
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

        public void ShowEvolveButton()
        {
            evolveButton.SetActive(true);
        }

        public void HideEvolveButton()
        {
            evolveButton.SetActive(false);
        }

        public void OnEvolveButtonPressed()
        {
            hologramCanvas.HideCanvas();
            HideEvolveButton();
            OnEvolveDigeomonAction?.Invoke(digeomonModel, digeomon.evolution);
        }
    }
}