using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIBulletCollision : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject infoPanel;
    public GameObject optionsPanel;
    public GameObject countdownTextObj; 
    public string gameSceneName = "GameScene";
    public float countdownTime = 5f; 

    private Text countdownText; 

    private void Start()
    {
        if (countdownTextObj != null)
        {
            countdownText = countdownTextObj.GetComponent<Text>();
            countdownTextObj.SetActive(false); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
        {
            return;
        }

        Collider buttonCollider = this.GetComponent<Collider>(); 

        if (buttonCollider.CompareTag("PlayButton")) 
        {
            StartCoroutine(StartGameCountdown());
        }
        else if (buttonCollider.CompareTag("InfoButton")) 
        {
            //ShowPanel(infoPanel);
            infoPanel.SetActive(true);
        }
        else if (buttonCollider.CompareTag("OptionsButton")) 
        {
            ShowPanel(optionsPanel);
        }
        else if (buttonCollider.CompareTag("BackButton")) 
        {
            ShowPanel(mainMenuPanel);
        }

        Destroy(other.gameObject); 
    }

    private void ShowPanel(GameObject panelToShow)
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (infoPanel) infoPanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);

        if (panelToShow)
        {
            panelToShow.SetActive(true);
        }
    }

    private IEnumerator StartGameCountdown()
    {
        ShowPanel(null); 
        countdownTextObj.SetActive(true); 

        float timeLeft = countdownTime;
        while (timeLeft > 0)
        {
            countdownText.text = timeLeft.ToString("0"); 
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        countdownText.text = "GO!"; 
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(gameSceneName); 
    }
}
