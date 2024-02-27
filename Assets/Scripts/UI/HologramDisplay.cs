using TMPro;
using UnityEngine;

namespace UI
{
    public class HologramDisplay : MonoBehaviour
    {
        [SerializeField] private DigeomonData digeomonData;

        [SerializeField] private GameObject informationPanel;
        [SerializeField] private GameObject digeomonModel;
        [SerializeField] private GameObject digeomonShape;

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private RectTransform arrowRT;

        public void SetContentText(string content)
        {
            contentText.text = content;
        }

        public void ToggleInformationPanel()
        {
            informationPanel.SetActive(!informationPanel.activeInHierarchy);
        }
    }
}