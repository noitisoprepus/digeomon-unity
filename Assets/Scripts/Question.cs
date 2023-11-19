using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "question", menuName = "QuizData/QuestionObject", order = 1)]
public class Question : ScriptableObject
{
    public string question;
    public string[] choices = new string[4];
    public int answerIndex;
}
