using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string VolumeKey = "GameVolume"; 
    public float volumeStep = 0.1f; 

    private void Start()
    {

        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        AudioListener.volume = savedVolume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet")) return; 

        if (this.CompareTag("IncreaseSound"))
        {
            IncreaseVolume();
        }
        else if (this.CompareTag("DecreaseSound"))
        {
            DecreaseVolume();
        }

        Destroy(other.gameObject); 
    }

    public void IncreaseVolume()
    {
        float newVolume = Mathf.Clamp(AudioListener.volume + volumeStep, 0f, 1f); 
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat(VolumeKey, newVolume); 
        PlayerPrefs.Save(); 
        Debug.Log("Ses arttý: " + newVolume);
    }

    public void DecreaseVolume()
    {
        float newVolume = Mathf.Clamp(AudioListener.volume - volumeStep, 0f, 1f);
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat(VolumeKey, newVolume);
        PlayerPrefs.Save();
        Debug.Log("Ses azaldý: " + newVolume);
    }
}
