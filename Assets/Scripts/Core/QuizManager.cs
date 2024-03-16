using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class QuizManager : MonoBehaviour
    {
        public delegate void AnswerCorrect();
        public static event AnswerCorrect OnAnswerCorrectAction;

        public delegate void AnswerIncorrect();
        public static event AnswerIncorrect OnAnswerIncorrectAction;

        [SerializeField] DigeomonCaptureData digeomonCaptureData;

        [Header("Quiz Audio")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip quizMusic;
        [SerializeField] private AudioClip quizSuccessSFX;
        [SerializeField] private AudioClip quizFailSFX;

        [HideInInspector] public QuizUI quizUI;

        private AudioSource quizAudioSource;
        private AudioClip prevMusic;
        private JournalManager journalManager;
        private DigeomonData targetDigeomon;
        private QuizData currQuiz;
        private List<QuestionData> currQuestions;
        private List<int> userAnswers;
        private int currQIndex;
        private int score;

        private void Awake()
        {
            quizAudioSource = GetComponent<AudioSource>();
            journalManager = GameManager.Instance.gameObject.GetComponent<JournalManager>();
        }

        private void OnEnable()
        {
            DigeomonCaptureData.OnDigeomonCapture += journalManager.AddDigeomon;
            
            ScannerUI.OnCaptureAction += StartQuiz;
            
            QuizUI.OnAnswerAction += CheckAnswer;
            QuizUI.OnStartRecapAction += StartRecap;
            QuizUI.OnNextRecapAction += NextRecapQuestion;
            QuizUI.OnGoToSceneRequested += GameManager.Instance.GoToScene;
        }

        private void OnDisable()
        {
            DigeomonCaptureData.OnDigeomonCapture -= journalManager.AddDigeomon;
            
            ScannerUI.OnCaptureAction -= StartQuiz;

            QuizUI.OnAnswerAction -= CheckAnswer;
            QuizUI.OnStartRecapAction -= StartRecap;
            QuizUI.OnNextRecapAction -= NextRecapQuestion;
            QuizUI.OnGoToSceneRequested -= GameManager.Instance.GoToScene;
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name.Equals("Sandbox"))
                StartQuiz();
        }

        public void StartQuiz()
        {
            targetDigeomon = PersistentData.targetDigeomon;
            currQuiz = QuizShuffler.GenerateQuiz(targetDigeomon.quiz, 3, 2);
            currQuestions = new List<QuestionData>(currQuiz.questions);
            score = 0;
            currQIndex = 0;
            userAnswers = new List<int>();
            
            quizUI.OpenPanel();
            quizUI.ShowQuestion(currQuestions[currQIndex]);

            musicSource.Stop();
            prevMusic = musicSource.clip;
            musicSource.clip = quizMusic;
            musicSource.Play();
        }

        private void NextQuestion()
        {
            currQIndex++;

            if (currQIndex == currQuestions.Count)
            {
                bool success = score >= currQuiz.passingScore;
                quizUI.ShowScoreDialog(score, success);

                if (success)
                {
                    digeomonCaptureData.CaptureDigeomon(targetDigeomon);
                    
                    quizAudioSource.clip = quizSuccessSFX;
                    quizAudioSource.Play();
                }
                else
                {
                    quizAudioSource.clip = quizFailSFX;
                    quizAudioSource.Play();
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
                musicSource.Stop();
                musicSource.clip = prevMusic;
                musicSource.Play();

                quizUI.OnHomeButtonPressed();
                return;
            }

            quizUI.ShowRecapQuestion(currQuestions[currQIndex], userAnswers[currQIndex]);
        }

        private void CheckAnswer(int index)
        {
            userAnswers.Add(index);
            if (index == currQuestions[currQIndex].answerIndex)
            {
                OnAnswerCorrectAction?.Invoke();
                score++;
            }
            else
            {
                OnAnswerIncorrectAction?.Invoke();
            }
            NextQuestion();
        }
    }
}