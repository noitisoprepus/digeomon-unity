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
    public Color normalCol;
    public Color correctCol;
    public Color wrongCol;

    private TextMeshProUGUI choiceAText;
    private TextMeshProUGUI choiceBText;
    private TextMeshProUGUI choiceCText;
    private TextMeshProUGUI choiceDText;
    private List<Question> currQuestions;
    private List<int> userAnswers;
    private Image[] choiceButtonImages = new Image[4];
    private int currQIndex;
    private int score;

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
        {
            currQuestions = new List<Question>(quiz.questions);
        }
        else if (useCustom)
        {
            if (isRandom)
            {
                List<Question> tempList = new List<Question>(questions);
                for (int i = 0; i < tempList.Count; i++)
                {
                    Question temp = tempList[i];
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
            return;
        }

        ShowQuestion(currQuestions[currQIndex]);
    }

    public void StartRecap()
    {
        currQIndex = 0;
        ShowRecapQuestion(currQuestions[currQIndex]);
        questionBox.SetActive(true);
        choicesBox.SetActive(true);
    }

    private void ShowRecapQuestion(Question q)
    {
        ShowQuestion(q);
        
        choiceAButton.interactable = false;
        choiceBButton.interactable = false;
        choiceCButton.interactable = false;
        choiceDButton.interactable = false;

        for (int i = 0; i < 4; i++)
            choiceButtonImages[i].color = normalCol;

        if (q.answerIndex == userAnswers[currQIndex])
            choiceButtonImages[userAnswers[currQIndex]].color = correctCol;
        else
        {
            choiceButtonImages[userAnswers[currQIndex]].color = wrongCol;
            choiceButtonImages[q.answerIndex].color = correctCol;
        }
    }

    private void NextRecapQuestion()
    {
        currQIndex++;

        if (currQIndex == currQuestions.Count)
        {
            panel.SetActive(false);
            return;
        }

        ShowRecapQuestion(currQuestions[currQIndex]);
    }

    public void OnAnswerButtonPressed(int index)
    {
        userAnswers.Add(index);
        if (index == currQuestions[currQIndex].answerIndex)
            score++;
        NextQuestion();
    }

    public void OnRecapButtonPressed()
    {
        StartRecap();
    }

    public void OnNextRecapButtonPressed()
    {
        NextRecapQuestion();
    }
}
