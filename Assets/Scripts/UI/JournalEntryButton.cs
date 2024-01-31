using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class JournalEntryButton : MonoBehaviour
    {
        [SerializeField] private Image previewImage;
        [SerializeField] private GameObject checkmarkObj;

        public void SetPreviewImage(Sprite image, bool isCaught)
        {
            previewImage.sprite = image;

            previewImage.color = isCaught ? Color.white : Color.black;
            checkmarkObj.SetActive(!isCaught);
        }
    }
}