using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HologramCanvas : MonoBehaviour
    {
        [Header("Information Panel GUI")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private RectTransform arrowLeftRT;
        [SerializeField] private RectTransform arrowRightRT;
        [SerializeField] private Image contextualImage;

        private List<InformationalData> informationalContent;
        private List<string> currContent;
        private List<Sprite> currImages;
        private InformationalData currData;
        private Sequence arrowLeftSequence;
        private Sequence arrowRightSequence;
        private int currDataIndex;
        private int currContentIndex;

        private void OnEnable()
        {
            QuizUI.OnQuizBeginAction += HideCanvas;
            QuizUI.OnQuizConcludeAction += ShowCanvas;
        }

        private void OnDisable()
        {
            QuizUI.OnQuizBeginAction -= HideCanvas;
            QuizUI.OnQuizConcludeAction -= ShowCanvas;
        }

        private void Start()
        {
            arrowLeftSequence = DOTween.Sequence();
            arrowLeftSequence.Append(arrowLeftRT.DOAnchorPosX(-0.06f, 0.5f));
            arrowLeftSequence.Append(arrowLeftRT.DOAnchorPosX(-0.08f, 0.5f));
            arrowLeftSequence.SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
            arrowLeftSequence.Play();

            arrowRightSequence = DOTween.Sequence();
            arrowRightSequence.Append(arrowRightRT.DOAnchorPosX(-0.34f, 0.5f));
            arrowRightSequence.Append(arrowRightRT.DOAnchorPosX(-0.32f, 0.5f));
            arrowRightSequence.SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
            arrowRightSequence.Play();
        }

        private void ShowContent(int index)
        {
            contentText.text = currContent[index];
            if (currImages[index] != null)
            {
                contextualImage.transform.localScale = new Vector3(1f, 0f, 1f);
                contextualImage.sprite = currImages[index];
                contextualImage.transform.DOScaleY(1f, 0.75f).SetEase(Ease.OutQuart);
            }
            else
            {
                contextualImage.transform.DOScaleY(0f, 0.6f).SetEase(Ease.OutExpo);
            }
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
            currImages = new List<Sprite>(informationalData.images);
            currContentIndex = 0;

            titleText.text = informationalData.title;
            ShowContent(currContentIndex);
        }

        public void OnNextSlideTriggered()
        {
            currContentIndex++;
            if (currContentIndex >= currContent.Count)
            {
                StartPresentation();
                return;
            }

            ShowContent(currContentIndex);
        }

        public void OnPreviousSlideTriggered()
        {
            currContentIndex--;
            if (currContentIndex < 0)
            {
                currDataIndex--;
                if (currDataIndex < 0)
                    currDataIndex = informationalContent.Count - 1;
                StartPresentation();
                return;
            }

            ShowContent(currContentIndex);
        }

        public void ShowCanvas()
        {
            transform.DOScaleY(1f, 0.75f).SetEase(Ease.OutQuart);
        }

        public void HideCanvas()
        {
            transform.DOScaleY(0f, 0.6f).SetEase(Ease.OutExpo);
        }

        // Testing
        InformationalData testData;

        public void InitializeContent(InformationalData contentData)
        {
            testData = contentData;
            ShowPresentation(testData);
        }

        public void NextContent()
        {
            currContentIndex++;
            if (currContentIndex >= currContent.Count)
                ShowPresentation(testData);
            ShowContent(currContentIndex);
        }
    }
}