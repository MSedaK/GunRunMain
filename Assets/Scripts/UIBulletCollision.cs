using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIBulletCollision : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject infoPanel;
    public GameObject optionsPanel;
    public GameObject countdownTextObj;
    public string gameSceneName = "GameScene"; 
    public int gameSceneIndex = -1; 
    public float countdownTime = 5f;

    private TMP_Text countdownText; 

    private void Start()
    {
        if (countdownTextObj != null)
        {
            countdownText = countdownTextObj.GetComponent<TMP_Text>(); 
            countdownTextObj.SetActive(false); 
        }
        else
        {
            Debug.LogError("countdownTextObj is NULL! Assign it in the Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet")) return;

        Debug.Log("Bullet hit: " + this.gameObject.name); 

        if (gameObject.CompareTag("PlayButton")) 
        {
            StartCoroutine(StartGameCountdown());
        }
        else if (gameObject.CompareTag("InfoButton"))
        {
            ShowPanel(infoPanel);
        }
        else if (gameObject.CompareTag("OptionsButton"))
        {
            ShowPanel(optionsPanel);
        }
        else if (gameObject.CompareTag("BackButton"))
        {
            ShowPanel(mainMenuPanel);
        }

        //Destroy(other.gameObject); 
    }

    private void ShowPanel(GameObject panelToShow)
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (infoPanel) infoPanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);

        if (panelToShow) panelToShow.SetActive(true);
    }

    private IEnumerator StartGameCountdown()
    {
        Debug.Log("Countdown started");

        ShowPanel(null); 
        countdownTextObj.SetActive(true); 

        if (countdownText == null)
        {
            Debug.LogError("countdownText is NULL! TMP_Text component is not assigned.");
            yield break;
        }

        float timeLeft = countdownTime;
        while (timeLeft > 0)
        {
            countdownText.text = timeLeft.ToString("0");
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        Debug.Log("Loading scene...");

        if (gameSceneIndex >= 0)
        {
            SceneManager.LoadScene(gameSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}
