using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject restartCanvas;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText; 

    private float gameTimer;
    public float gameDuration = 360f;
    private int score = 0; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameTimer = gameDuration; 
        UpdateTimerUI();
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            gameTimer -= Time.deltaTime;
            UpdateTimerUI();

            if (gameTimer <= 0)
            {
                gameTimer = 0;
                GameOver();
            }
        }
    }

    public void AddScore(int damage)
    {
        score += damage; 
        scoreText.text = score.ToString(); 
    }

    private void UpdateTimerUI()
    {
        timerText.text = Mathf.Ceil(gameTimer).ToString();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        restartCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
