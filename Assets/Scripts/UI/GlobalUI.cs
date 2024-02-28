using DG.Tweening;
using TMPro;
using UnityEngine;

public class GlobalUI : MonoBehaviour
{
    [Header("Custom Dialog Text")]
    [SerializeField] private GameObject customDialog;
    [SerializeField] private RectTransform customDialogPanel;
    [SerializeField] private TextMeshProUGUI customDialogText;

    public void ShowCustomDialog(string customText)
    {
        customDialogText.text = customText;
        customDialog.SetActive(true);

        customDialogPanel.localScale = Vector3.zero;
        customDialogPanel.DOScale(1f, 0.6f).SetEase(Ease.OutQuad);
    }
}
