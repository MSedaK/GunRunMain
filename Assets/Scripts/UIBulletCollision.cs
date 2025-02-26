using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIBulletCollision : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject infoPanel;
    public GameObject optionsPanel;
    public TextMeshProUGUI countdownText; // Geri sayým için UI metni

    private void Start()
    {
        if (countdownText) countdownText.gameObject.SetActive(false); // Geri sayým baþlangýçta gizli
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
        else if (gameObject.CompareTag("TrainingLabButton"))
        {
            Debug.Log("Loading AimLab scene...");
            SceneManager.LoadScene("AimLab");
        }
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
        if (countdownText)
        {
            countdownText.gameObject.SetActive(true);

            for (int i = 5; i > 0; i--)
            {
                countdownText.text = "Starting in " + i + "...";
                yield return new WaitForSeconds(1f);
            }

            countdownText.text = "GO!";
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("MainGame");
        }
        else
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("MainGame");
        }
    }
}
