using TMPro;
using UnityEngine;

namespace UI
{
    public class NameScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;

        public void SetName(string name)
        {
            nameText.text = name;
        }
    }
}