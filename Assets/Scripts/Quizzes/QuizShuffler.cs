using System;
using System.Collections.Generic;
using Random = System.Random;

public static class QuizShuffler
{
    // Fisher-Yates shuffle algorithm
    private static void Shuffle<T>(IList<T> list, Random rng)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static QuizData GenerateQuiz(QuizData quiz, int items, int passingScore)
    {
        Random rng = new Random();
        Shuffle(quiz.questions, rng);
        foreach (QuestionData question in quiz.questions)
        {
            string correctAnswer = question.choices[question.answerIndex];
            Shuffle(question.choices, rng);
            question.answerIndex = Array.IndexOf(question.choices, correctAnswer);
        }

        QuizData generatedQuiz = new QuizData
        {
            passingScore = passingScore
        };
        for (int i = 0; i < items; i++)
        {
            generatedQuiz.questions.Add(quiz.questions[i]);
        }
        return generatedQuiz;
    }
}
