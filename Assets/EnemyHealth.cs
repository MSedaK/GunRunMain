using System;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public static event Action OnEnemyKilled;

    public NavMeshAgent agent;
    public float totalHealth = 100f;
    private float currentHealth;

    public float baseDamage = 50f;
    public float headshotMultiplier = 2f;

    public Collider headCollider;
    public GameObject deathVFX;
    public GameObject floatingTextPrefab;

    void Start()
    {
        currentHealth = totalHealth;
    }

    public void TakeDamage(float damage, Collider hitCollider)
    {
        if (hitCollider == headCollider)
        {
            damage *= headshotMultiplier;
        }

        currentHealth -= damage;
        ShowFloatingText(damage, hitCollider);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ShowFloatingText(float damage, Collider hitCollider)
    {
        if (floatingTextPrefab != null)
        {
            Vector3 hitPosition = hitCollider.ClosestPointOnBounds(transform.position);
            GameObject damageText = Instantiate(floatingTextPrefab, hitPosition, Quaternion.identity);
            TextMeshPro textMesh = damageText.GetComponent<TextMeshPro>();

            if (textMesh != null)
            {
                textMesh.text = damage.ToString();
            }
        }
    }

    private void Die()
    {
        OnEnemyKilled?.Invoke();

        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Enemy çarptý! Oyun durdu.");
            GameManager.Instance.GameOver();
        }
    }
}
