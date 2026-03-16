using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public GameObject endPanel;
    public Text finalScoreText;
    public Text finalTimeText;

    void Start()
    {
        endPanel.SetActive(false);
    }

    public void ShowEndScreen()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0f;
        GameManager.instance.StopTimer();
        finalScoreText.text = "Final Score: " + GameManager.instance.score;
        int minutes = Mathf.FloorToInt(GameManager.instance.GetTime() / 60f);
        int seconds = Mathf.FloorToInt(GameManager.instance.GetTime() % 60f);
        finalTimeText.text = "Time: " + minutes + "m " + seconds + "s";
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GameManager.instance.health = 3;
        GameManager.instance.score = 0;
        GameManager.instance.timer = 0f;
        GameManager.instance.levelStartTime = 0f;
        GameManager.instance.timerRunning = true;
        SceneManager.LoadScene("Level1");
    }
}