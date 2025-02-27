using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject restartCanvas;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject timeAndScorePanel;
    public AudioSource backgroundMusic;

    private float gameTimer;
    public float gameDuration = 150f;
    private int score = 0;
    private bool isGameOver = false;

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

        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }

    private void Update()
    {
        if (!isGameOver) 
        {
            gameTimer -= Time.deltaTime;
            UpdateTimerUI();

            if (gameTimer <= 0)
            {
                gameTimer = 0;
                GameOver(null); 
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

    public void GameOver(Collider hitCollider)
    {
        if (isGameOver) return;
        isGameOver = true;

        EnemyBehavior[] enemies = FindObjectsOfType<EnemyBehavior>();
        foreach (EnemyBehavior enemy in enemies)
        {
            enemy.gameObject.GetComponentInChildren<Animator>().speed = 0;
            enemy.DisableColliders();
            Debug.Log($"Enemy {enemy.name} is disabled.");
            //enemy.enabled = false;
            enemy.Stop();
            //if (enemy.GetComponent<Rigidbody>() != null)
            //{
            //    enemy.GetComponent<Rigidbody>().isKinematic = true;
            //    enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //}
        }

        if (timeAndScorePanel != null)
        {
            timeAndScorePanel.SetActive(false);
        }

        restartCanvas.SetActive(true);

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        if (hitCollider != null)
        {
            Debug.Log("Game Over! Oyuncu " + hitCollider.name + " tarafından vuruldu.");
        }
        else
        {
            Debug.Log("Game Over! Süre doldu.");
        }
    }

    public void RestartGame()
    {
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
