using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        GameManager.instance.health = 3;
        GameManager.instance.score = 0;
        GameManager.instance.timer = GameManager.instance.levelStartTime;
        GameManager.instance.timerRunning = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        GameManager.instance.health = 3;
        GameManager.instance.score = 0;
        GameManager.instance.timer = 0f;
        GameManager.instance.timerRunning = true;
        SceneManager.LoadScene("Main Menu Scene");
    }
}