using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetBoardHealth : MonoBehaviour
{
    public float totalHealth = 100f;
    private float currentHealth;

    public GameObject floatingTextPrefab;
    public AudioClip hitSound; 
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = totalHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage, Vector3 hitPoint)
    {
        currentHealth -= damage;

        ShowFloatingText(damage, hitPoint);

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    private void ShowFloatingText(float damage, Vector3 hitPoint)
    {
        if (floatingTextPrefab != null)
        {
            GameObject damageText = Instantiate(floatingTextPrefab, hitPoint, Quaternion.identity);
            TextMeshPro textMesh = damageText.GetComponent<TextMeshPro>();

            if (textMesh != null)
            {
                textMesh.text = damage.ToString();
            }

            Destroy(damageText, 1.5f); 
        }
    }
}
