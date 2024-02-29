using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class HologramDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject digeomonModel;
        [SerializeField] private GameObject digeomonShape;
        [SerializeField] private GameObject informationPanel;

        private bool infoToggle = false;

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
    }
}