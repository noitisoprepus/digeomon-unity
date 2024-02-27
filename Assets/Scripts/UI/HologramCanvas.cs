using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HologramCanvas : MonoBehaviour
    {
        [Header("Information Panel GUI")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private RectTransform arrowRT;

        [Header("Informational Data")]
        [SerializeField] private List<InformationalData> informationalContent;

        private void Start()
        {
            // Show first slide right away
        }

        private void SetContentText(string content)
        {
            contentText.text = content;
        }

        public void GoToNextSlide()
        {

        }
    }
}