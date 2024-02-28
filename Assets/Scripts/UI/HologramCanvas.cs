using DG.Tweening;
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

        private Sequence arrowSequence;
        private InformationalData currData;
        private List<string> currContent;
        private int currDataIndex = 0;
        private int currContentIndex = 0;

        private void Start()
        {
            arrowSequence = DOTween.Sequence();
            arrowSequence.Append(arrowRT.DOAnchorPosX(-0.08f, 0.5f));
            arrowSequence.Append(arrowRT.DOAnchorPosX(-0.12f, 0.5f));
            arrowSequence.SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
            arrowSequence.Play();

            StartPresentation();
        }

        private void ShowContent(string content)
        {
            contentText.text = content;
        }

        private void StartPresentation()
        {
            currData = informationalContent[currDataIndex];
            ShowPresentation(currData);

            currDataIndex++;
            if (currDataIndex >= informationalContent.Count)
                currDataIndex = 0;  // Reset back to start, creating a loop.
        }

        private void ShowPresentation(InformationalData informationalData)
        {
            currContent = new List<string>(informationalData.content);
            currContentIndex = 0;

            titleText.text = informationalData.title;
            ShowContent(currContent[currContentIndex]);
        }

        public void OnNextSlideTriggered()
        {
            currContentIndex++;
            if (currContentIndex >= currContent.Count)
                StartPresentation();

            ShowContent(currContent[currContentIndex]);
        }
    }
}