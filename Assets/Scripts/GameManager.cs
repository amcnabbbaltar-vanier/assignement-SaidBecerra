using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public int health = 3;
    public float timer = 0f;
    public bool timerRunning = true;

    private Text scoreText;
    private Text healthText;
    private Text timerText;
    public float levelStartTime = 0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scoreText = GameObject.Find("ScoreText")?.GetComponent<Text>();
        healthText = GameObject.Find("HealthText")?.GetComponent<Text>();
        timerText = GameObject.Find("TimerText")?.GetComponent<Text>();
        UpdateUI();
    }

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            if (timerText != null)
                timerText.text = "Time: " + Mathf.FloorToInt(timer) + "s";
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            health = 3;
            score = 0;
            timer = 0f;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TakeTrapDamage()
    {
        health--;
        UpdateUI();
        if (health <= 0)
        {
            health = 3;
            score = 0;
            timer = 0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void LoadNextLevel()
    {
        health = 3;
        levelStartTime = timer;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public float GetTime()
    {
        return timer;
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (healthText != null) healthText.text = "Health: " + health;
    }
}