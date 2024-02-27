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
            // Switch between digeomonModel and digeomonShape
        }

        public void ToggleInformationPanel()
        {
            informationPanel.SetActive(!informationPanel.activeInHierarchy);
        }
    }
}