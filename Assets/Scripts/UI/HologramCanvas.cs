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

        private List<InformationalData> informationalContent;
        private List<string> currContent;
        private InformationalData currData;
        private Sequence arrowSequence;
        private int currDataIndex;
        private int currContentIndex;

        private void Start()
        {
            arrowSequence = DOTween.Sequence();
            arrowSequence.Append(arrowRT.DOAnchorPosX(-0.08f, 0.5f));
            arrowSequence.Append(arrowRT.DOAnchorPosX(-0.12f, 0.5f));
            arrowSequence.SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
            arrowSequence.Play();
        }

        private void ShowContent(string content)
        {
            contentText.text = content;
        }

        public void InitializeInformationalData(List<InformationalData> informationalDatas)
        {
            currDataIndex = 0;
            currContentIndex = 0;
            informationalContent = new List<InformationalData>(informationalDatas);
            StartPresentation();
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

        // Testing
        int testIndex = 0;
        List<string> testContent;
        InformationalData testData;

        public void InitializeContent(InformationalData contentData)
        {
            testData = contentData;
            StartContent();
        }

        public void StartContent()
        {
            testIndex = 0;
            testContent = new List<string>(testData.content);
            titleText.text = testData.title;
            ShowContent(testContent[testIndex]);
        }

        public void NextContent()
        {
            testIndex++;
            if (testIndex >= testContent.Count)
                StartContent();
            ShowContent(testContent[testIndex]);
        }
    }
}