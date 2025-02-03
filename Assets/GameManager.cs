using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject restartCanvas; 
    private float gameTimer = 0f;
    public float gameDuration = 45f; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            gameTimer += Time.deltaTime;

            if (gameTimer >= gameDuration)
            {
                GameOver(); 
            }
        }
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
