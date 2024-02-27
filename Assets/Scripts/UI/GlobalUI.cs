using TMPro;
using UnityEngine;

public class GlobalUI : MonoBehaviour
{
    [Header("Custom Dialog Text")]
    [SerializeField] private GameObject customDialog;
    [SerializeField] private TextMeshProUGUI customDialogText;

    public void ShowCustomDialog(string customText)
    {
        customDialogText.text = customText;
        customDialog.SetActive(true);
    }
}
