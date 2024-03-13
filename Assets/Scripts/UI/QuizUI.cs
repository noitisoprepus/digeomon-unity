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

        public delegate void QuizBeginDelegate();
        public static event QuizBeginDelegate OnQuizBeginAction;

        public delegate void QuizConcludeDelegate();
        public static event QuizConcludeDelegate OnQuizConcludeAction;

        public delegate void EvolutionSuccess();
        public static event EvolutionSuccess OnEvolutionSuccessAction;

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

        private void ResetUI()
        {
            questionBox.SetActive(true);
            choicesBox.SetActive(true);

            scoreBoxRect.localScale = new Vector3(1f, 0f, 1f);
            scoreBoxRect.anchoredPosition = Vector2.zero;

            choiceCButton.interactable = true;
            choiceBButton.interactable = true;
            choiceDButton.interactable = true;
            choiceAButton.interactable = true;

            for (int i = 0; i < 4; i++)
                choiceButtonImages[i].color = normalCol;
        }

        public void OpenPanel()
        {
            ResetUI();
            OnQuizBeginAction?.Invoke();
            panel.transform.DOScaleY(1f, 0.75f).SetEase(Ease.OutQuart);
        }

        public void ClosePanel()
        {
            panel.transform.DOScaleY(0f, 0.6f).SetEase(Ease.OutExpo);
        }

        public void ShowQuestion(QuestionData q)
        {
            questionText.text = q.question;
            choiceAText.text = "A. " + q.choices[0];
            choiceBText.text = "B. " + q.choices[1];
            choiceCText.text = "C. " + q.choices[2];
            choiceDText.text = "D. " + q.choices[3];
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
            scoreBoxRect.anchoredPosition = Vector2.zero;
            scoreBoxRect.DOScaleY(1f, 0.75f).SetEase(Ease.OutQuart);
        }

        public void OnRecapButtonPressed()
        {
            scoreBoxRect.DOAnchorPosY(0.34f, 0.75f).SetEase(Ease.OutQuad);
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
            if (PersistentData.toEvolve)
                OnEvolutionSuccessAction?.Invoke();
            ClosePanel();
            OnQuizConcludeAction?.Invoke();
        }
    }
}