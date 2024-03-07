using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuizUI : MonoBehaviour
    {
        public delegate void AnswerDelegate(int answerIndex);
        public static event AnswerDelegate OnAnswerAction;

        public delegate void StartRecapDelegate();
        public static event StartRecapDelegate OnStartRecapAction;

        public delegate void NextRecapDelegate();
        public static event NextRecapDelegate OnNextRecapAction;

        public static event Action<string> OnGoToSceneRequested;

        [Header("Quiz GUI")]
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject questionBox;
        [SerializeField] private GameObject choicesBox;
        [SerializeField] private Button choiceAButton;
        [SerializeField] private Button choiceBButton;
        [SerializeField] private Button choiceCButton;
        [SerializeField] private Button choiceDButton;
        [SerializeField] private TextMeshProUGUI questionText;

        [Header("Review GUI")]
        [SerializeField] private RectTransform scoreBoxRect;
        [SerializeField] private GameObject reviewButton;
        [SerializeField] private GameObject nextReviewButton;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Color normalCol;
        [SerializeField] private Color correctCol;
        [SerializeField] private Color wrongCol;

        private TextMeshProUGUI choiceAText;
        private TextMeshProUGUI choiceBText;
        private TextMeshProUGUI choiceCText;
        private TextMeshProUGUI choiceDText;
        private Image[] choiceButtonImages = new Image[4];

        private void Awake()
        {
            choiceAText = choiceAButton.GetComponentInChildren<TextMeshProUGUI>();
            choiceBText = choiceBButton.GetComponentInChildren<TextMeshProUGUI>();
            choiceCText = choiceCButton.GetComponentInChildren<TextMeshProUGUI>();
            choiceDText = choiceDButton.GetComponentInChildren<TextMeshProUGUI>();

            choiceButtonImages[0] = choiceAButton.GetComponent<Image>();
            choiceButtonImages[1] = choiceBButton.GetComponent<Image>();
            choiceButtonImages[2] = choiceCButton.GetComponent<Image>();
            choiceButtonImages[3] = choiceDButton.GetComponent<Image>();
        }

        private void Start()
        {
            scoreBoxRect.anchoredPosition = new Vector2(0f, 256f);
        }

        public void ShowQuestion(QuestionData q)
        {
            questionText.text = q.question;
            choiceAText.text = "A. " + q.choices[0];
            choiceBText.text = "B. " + q.choices[1];
            choiceCText.text = "C. " + q.choices[2];
            choiceDText.text = "D. " + q.choices[3];

            panel.SetActive(true);
        }

        public void OnAnswerButtonPressed(int index)
        {
            OnAnswerAction?.Invoke(index);
        }

        public void ShowScoreDialog(int score)
        {
            questionBox.SetActive(false);
            choicesBox.SetActive(false);
            reviewButton.SetActive(true);
            nextReviewButton.SetActive(false);

            scoreText.text = "SCORE: " + score;
            scoreBoxRect.DOAnchorPosY(-224f, 0.6f).SetEase(Ease.OutQuad);
        }

        public void OnRecapButtonPressed()
        {
            OnStartRecapAction?.Invoke();
            questionBox.SetActive(true);
            choicesBox.SetActive(true);
            reviewButton.SetActive(false);
            nextReviewButton.SetActive(true);
        }

        public void ShowRecapQuestion(QuestionData q, int userAnswer)
        {
            ShowQuestion(q);

            choiceAButton.interactable = false;
            choiceBButton.interactable = false;
            choiceCButton.interactable = false;
            choiceDButton.interactable = false;

            for (int i = 0; i < 4; i++)
                choiceButtonImages[i].color = normalCol;

            if (q.answerIndex == userAnswer)
                choiceButtonImages[userAnswer].color = correctCol;
            else
            {
                choiceButtonImages[userAnswer].color = wrongCol;
                choiceButtonImages[q.answerIndex].color = correctCol;
            }
        }

        public void OnNextRecapButtonPressed()
        {
            OnNextRecapAction?.Invoke();
        }

        public void OnHomeButtonPressed()
        {
            OnGoToSceneRequested?.Invoke("Scanner");
        }
    }
}