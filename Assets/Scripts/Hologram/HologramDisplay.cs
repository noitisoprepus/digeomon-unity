using TMPro;
using UnityEngine;

namespace Hologram
{
    public class HologramDisplay : MonoBehaviour
    {
        [SerializeField] private DigeomonData digeomonData;

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI contentText;

        public void SetContentText(string content)
        {
            contentText.text = content;
        }
    }
}