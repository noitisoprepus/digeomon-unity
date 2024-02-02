using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "quiz", menuName = "QuizData/QuizObject", order = 1)]
public class QuizData : ScriptableObject
{
    public string key;
    public List<QuestionData> questions;
    public int passingScore;
}