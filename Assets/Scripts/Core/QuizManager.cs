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

        public delegate void EvolutionSuccess();
        public static event EvolutionSuccess OnEvolutionSuccessAction;

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
            if (SceneManager.GetActiveScene().name.Equals("Sandbox"))
                StartQuiz();
        }

        public void StartQuiz()
        {
            targetDigeomon = PersistentData.targetDigeomon;
            currQuiz = QuizShuffler.GenerateQuiz(targetDigeomon.quiz, 3, 2);
            userAnswers = new List<int>();
            currQIndex = 0;
            currQuestions = new List<QuestionData>(currQuiz.questions);
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
                quizUI.ShowScoreDialog(score);

                if (score >= currQuiz.passingScore)
                {
                    digeomonCaptureData.CaptureDigeomon(targetDigeomon);
                    
                    quizAudioSource.clip = quizSuccessSFX;
                    quizAudioSource.Play();

                    if (PersistentData.toEvolve)
                    {
                        OnEvolutionSuccessAction?.Invoke();
                        return;
                    }
                    
                    GameManager.Instance.ShowDialog("<color=#008A03>Capture Success</color>");
                }
                else
                {
                    quizAudioSource.clip = quizFailSFX;
                    quizAudioSource.Play();
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