using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [Header("Reference")]
    public QuizAPI api;

    [Header("UI")]
    public TMP_Text questionText;
    public Button resetButton;
    public GameObject actionButton;
    public QuestData currentQuest;

    public Button[] answerButtons;

    public TMP_Text[] answerTexts;

    private QuizQuestion[] questions;
    private int currentQuestion = 0;

    private int score = 0;
    private int jumlahBenar = 0;
    private int jumlahSalah = 0;

    private void Start()
    {
        StartCoroutine(
            api.GetQuestions(OnQuestionsLoaded)
        );
    }

    void OnQuestionsLoaded(QuizQuestion[] data)
    {
        questions = data;

        ShowQuestion();
    }

    void ShowQuestion()
    {
        // resetButton.gameObject.SetActive(false);
        actionButton.SetActive(false);
        if (currentQuestion >= questions.Length)
        {
            questionText.text = "Quiz selesai! Skor: " + score + " | Benar: " + jumlahBenar + " | Salah: " + jumlahSalah;
            foreach (var button in answerButtons)
            {
                button.gameObject.SetActive(false);
            }

            // resetButton.gameObject.SetActive(true);
            actionButton.SetActive(true);
            return;
        }

        QuizQuestion q = questions[currentQuestion];

        questionText.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < q.answers.Length; i++)
        {
            QuizAnswer answer = q.answers[i];

            answerButtons[i].gameObject.SetActive(true);

            answerTexts[i].text = answer.answer;

            answerButtons[i].onClick.RemoveAllListeners();

            answerButtons[i].onClick.AddListener(() =>
            {
                CheckAnswer(answer);
            });
        }
    }

    void CheckAnswer(QuizAnswer answer)
    {
        if (answer.is_correct)
        {
            Debug.Log("Benar");
            jumlahBenar++;
            score += 10;
        }
        else
        {
            Debug.Log("Salah");
            jumlahSalah++;
        }

        currentQuestion++;

        ShowQuestion();
    }

    public void ResetGame()
    {
        currentQuestion = 0;
        score = 0;
        jumlahBenar = 0;
        jumlahSalah = 0;

        ShowQuestion();
    }

    public void BackToMainGame()
    {
        if (currentQuest != null)
        {
            Debug.Log("Menyelesaikan quest: " + currentQuest.questName);
            QuestManager.instance.CompleteQuest(currentQuest);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Game");
    }
}