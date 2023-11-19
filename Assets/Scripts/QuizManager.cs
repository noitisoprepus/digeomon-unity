using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("Quiz Data (Check only one)")]
    [Header("Pre-made Quiz")]
    public bool useQuiz;
    public Quiz quiz;
    [Header("Custom Quiz")]
    public bool useCustom;
    public List<Question> questions;
    public bool isRandom;
    public int itemNumbers;
    public int passingScore;

    [Header("Quiz GUI")]
    public GameObject panel;
    public GameObject questionBox;
    public GameObject choicesBox;
    public GameObject scoreBox;
    public Button choiceAButton;
    public Button choiceBButton;
    public Button choiceCButton;
    public Button choiceDButton;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;

    private TextMeshProUGUI choiceAText;
    private TextMeshProUGUI choiceBText;
    private TextMeshProUGUI choiceCText;
    private TextMeshProUGUI choiceDText;
    private List<Question> currQuestions;
    private List<int> userAnswers;
    private int currQIndex;
    private int score;

    private void Awake()
    {
        choiceAText = choiceAButton.GetComponentInChildren<TextMeshProUGUI>();
        choiceBText = choiceBButton.GetComponentInChildren<TextMeshProUGUI>();
        choiceCText = choiceCButton.GetComponentInChildren<TextMeshProUGUI>();
        choiceDText = choiceDButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        panel.SetActive(false);
        scoreBox.SetActive(false);

        // For testing
        StartQuiz();
    }

    public void StartQuiz()
    {
        userAnswers = new List<int>();
        currQIndex = 0;

        if (useQuiz)
            currQuestions = new List<Question>(quiz.questions);
        else if (useCustom)
            currQuestions = new List<Question>(questions);

        ShowQuestion(currQuestions[currQIndex]);
    }

    private void ShowQuestion(Question q)
    {
        questionText.text = q.question;
        choiceAText.text = q.choices[0];
        choiceBText.text = q.choices[1];
        choiceCText.text = q.choices[2];
        choiceDText.text = q.choices[3];

        panel.SetActive(true);
    }

    private void NextQuestion()
    {
        currQIndex++;

        if (currQIndex == currQuestions.Count)
        {
            questionBox.SetActive(false);
            choicesBox.SetActive(false);
            scoreText.text = "SCORE: " + score;
            scoreBox.SetActive(true);
            // TODO: Review answers
            return;
        }

        ShowQuestion(currQuestions[currQIndex]);
    }

    public void OnAnswerButtonPressed(int index)
    {
        userAnswers.Add(index);
        if (index == currQuestions[currQIndex].answerIndex)
            score++;
        NextQuestion();
    }
}
