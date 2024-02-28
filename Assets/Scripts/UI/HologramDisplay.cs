using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class HologramDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject digeomonModel;
        [SerializeField] private GameObject digeomonShape;
        [SerializeField] private GameObject informationPanel;

        private void Start()
        {
            if (SceneManager.GetActiveScene().name.Equals("Sandbox"))
            {
                gameObject.SetActive(false);
                return;
            }
        }

        public void ToggleModel()
        {
            //if (digeomonModel.activeInHierarchy)
            //{
            //    digeomonModel.SetActive(false);
            //    digeomonShape.SetActive(true);
            //}
            //else
            //{
            //    digeomonShape.SetActive(false);
            //    digeomonModel.SetActive(true);
            //}
        }

        public void ToggleInformationPanel()
        {
            informationPanel.SetActive(!informationPanel.activeInHierarchy);
        }
    }
}