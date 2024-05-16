using UnityEngine;

[CreateAssetMenu(fileName = "question", menuName = "QuizData/QuestionObject", order = 1)]
public class QuestionData : ScriptableObject
{
    [TextArea(3, 5)]
    public string question;
    public Sprite image;
    public string[] choices = new string[4];
    public int answerIndex;
}
