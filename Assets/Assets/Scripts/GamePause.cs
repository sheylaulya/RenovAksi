using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseUI;

    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}