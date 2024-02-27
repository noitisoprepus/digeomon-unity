using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Core
{
    public class QuizManager : MonoBehaviour
    {
        [SerializeField] DigeomonCaptureData digeomonCaptureData;

        [Header("Quiz Data (Check only one)")]
        [Header("Pre-made Quiz")]
        [SerializeField] private bool useQuiz;
        [SerializeField] private QuizData quiz;
        [Header("Custom Quiz")]
        [SerializeField] private bool useCustom;
        [SerializeField] private List<QuestionData> questions;
        [SerializeField] private bool isRandom;
        [SerializeField] private int itemNumbers;
        [SerializeField] private int passingScore;

        private QuizUI quizUI;
        private JournalManager journalManager;
        private DigeomonData targetDigeomon;
        private List<QuestionData> currQuestions;
        private List<int> userAnswers;
        private int currQIndex;
        private int score;

        private void Awake()
        {
            quizUI = GetComponent<QuizUI>();
            journalManager = GameManager.Instance.gameObject.GetComponent<JournalManager>();
        }

        private void OnEnable()
        {
            DigeomonCaptureData.OnDigeomonCapture += journalManager.AddDigeomon;
            QuizUI.OnAnswerAction += CheckAnswer;
            QuizUI.OnStartRecapAction += StartRecap;
            QuizUI.OnNextRecapAction += NextRecapQuestion;
            QuizUI.OnGoToSceneRequested += GameManager.Instance.GoToScene;
        }

        private void OnDisable()
        {
            DigeomonCaptureData.OnDigeomonCapture -= journalManager.AddDigeomon;
            QuizUI.OnAnswerAction -= CheckAnswer;
            QuizUI.OnStartRecapAction -= StartRecap;
            QuizUI.OnNextRecapAction -= NextRecapQuestion;
            QuizUI.OnGoToSceneRequested -= GameManager.Instance.GoToScene;
        }

        private void Start()
        {
            targetDigeomon = PersistentData.targetDigeomon;
            quiz = targetDigeomon.quiz;
            useQuiz = true;
            StartQuiz();
        }

        public void StartQuiz()
        {
            userAnswers = new List<int>();
            currQIndex = 0;

            if (useQuiz)
            {
                currQuestions = new List<QuestionData>(quiz.questions);
            }
            else if (useCustom)
            {
                if (isRandom)
                {
                    List<QuestionData> tempList = new List<QuestionData>(questions);
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        QuestionData temp = tempList[i];
                        int randomIndex = Random.Range(i, tempList.Count);
                        tempList[i] = tempList[randomIndex];
                        tempList[randomIndex] = temp;
                    }

                    for (int i = 0; i < itemNumbers; i++)
                    {
                        currQuestions.Add(tempList[i]);
                    }
                }
            }

            quizUI.ShowQuestion(currQuestions[currQIndex]);
        }

        private void NextQuestion()
        {
            currQIndex++;

            if (currQIndex == currQuestions.Count)
            {
                quizUI.ShowScoreDialog(score);

                if (score >= quiz.passingScore)
                {
                    digeomonCaptureData.CaptureDigeomon(targetDigeomon);
                    GameManager.Instance.ShowDialog("<color=#008A03>Capture Success</color>");
                }
                else
                {
                    GameManager.Instance.ShowDialog("<color=#CF2E2E>Capture Failed</color>");
                }

                return;
            }

            quizUI.ShowQuestion(currQuestions[currQIndex]);
        }

        private void StartRecap()
        {
            currQIndex = 0;
            quizUI.ShowRecapQuestion(currQuestions[currQIndex], userAnswers[currQIndex]);
        }

        private void NextRecapQuestion()
        {
            currQIndex++;

            if (currQIndex == currQuestions.Count)
            {
                quizUI.OnHomeButtonPressed();
                return;
            }

            quizUI.ShowRecapQuestion(currQuestions[currQIndex], userAnswers[currQIndex]);
        }

        private void CheckAnswer(int index)
        {
            userAnswers.Add(index);
            if (index == currQuestions[currQIndex].answerIndex)
                score++;
            NextQuestion();
        }
    }
}