using System;

[Serializable]
public class QuizQuestion
{
    public int question_id;
    public string question;
    public QuizAnswer[] answers;
}