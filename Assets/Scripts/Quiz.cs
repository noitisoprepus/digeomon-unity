using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "quiz", menuName = "QuizData/QuizObject", order = 1)]
public class Quiz : ScriptableObject
{
    public string key;
    public List<Question> questions;
    public int passingScore;
}
