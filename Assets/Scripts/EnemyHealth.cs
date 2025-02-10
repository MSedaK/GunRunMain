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

    public float headshotMultiplier = 2f;
    public float bodyMultiplier = 1f;
    public float legsMultiplier = 0.7f;

    public Collider headCollider;
    public Collider bodyCollider;
    public Collider legsCollider;

    public GameObject deathVFX;
    public GameObject floatingTextPrefab;

    public AudioClip damageSFX;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = totalHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage, Collider hitCollider)
    {
        float adjustedDamage = 0f;

        if (hitCollider == headCollider)
        {
            adjustedDamage = damage * headshotMultiplier;
        }
        else if (hitCollider == bodyCollider)
        {
            adjustedDamage = damage * bodyMultiplier;
        }
        else if (hitCollider == legsCollider)
        {
            adjustedDamage = damage * legsMultiplier;
        }
        else
        {
            adjustedDamage = damage; 
        }

        currentHealth -= adjustedDamage;

        GameManager.Instance.AddScore((int)adjustedDamage);

        ShowFloatingText(adjustedDamage, hitCollider);

        if (currentHealth <= 0)
        {
            Die();
        }

        if (audioSource != null && damageSFX != null)
        {
            audioSource.PlayOneShot(damageSFX);
        }
    }

    private void ShowFloatingText(float damage, Collider hitCollider)
    {
        if (floatingTextPrefab != null)
        {
            Vector3 hitPosition = hitCollider.ClosestPointOnBounds(transform.position);
            GameObject damageText = Instantiate(floatingTextPrefab, hitPosition, Quaternion.identity);

            Vector3 moveDirection = agent.velocity.normalized;

            if (moveDirection.magnitude < 0.1f)
            {
                moveDirection = transform.forward;
            }

            damageText.transform.rotation = Quaternion.LookRotation(moveDirection);
            damageText.transform.position += new Vector3(0, 0.5f, 0);

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
}
