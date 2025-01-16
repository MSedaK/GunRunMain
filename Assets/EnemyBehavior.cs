using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float health = 200f; // Düþmanýn toplam caný
    public Transform targetPoint; // Hedef nokta
    public float moveSpeed = 3f; // Hareket hýzý
    public float stopDistance = 2f; // Hedefe yaklaþma mesafesi
    public GameObject deathEffect; // Ölüm efekti prefabý

    private Rigidbody rb;

    void Start()
    {
        // Rigidbody componentini al
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Kinematik olmalý
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (targetPoint == null) return;

        // Hedefe olan mesafeyi kontrol et
        float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);

        if (distanceToTarget > stopDistance)
        {
            // Hedefe doðru hareket et
            Vector3 direction = (targetPoint.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Ölüm efekti oluþtur
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Düþmaný yok et
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // Collider A: Direkt öldürür
        if (other.CompareTag("ColliderA"))
        {
            Die();
        }
        // Collider B, C, D: Hasar verir
        else if (other.CompareTag("ColliderB"))
        {
            TakeDamage(50f);
        }
        else if (other.CompareTag("ColliderC"))
        {
            TakeDamage(50f);
        }
        else if (other.CompareTag("ColliderD"))
        {
            TakeDamage(50f);
        }
    }
}
