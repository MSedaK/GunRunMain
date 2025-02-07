using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 50f;
    public AudioClip hitSound;  
    public GameObject damageEffectPrefab;  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("TargetBoard"))
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage, other);
                }
            }

            if (other.CompareTag("TargetBoard"))
            {
                AudioSource.PlayClipAtPoint(hitSound, other.transform.position);

                ShowDamageEffect(other);
            }

            Destroy(gameObject);
        }
    }

    private void ShowDamageEffect(Collider board)
    {
        GameObject damageEffect = Instantiate(damageEffectPrefab, board.transform.position, Quaternion.identity);
        damageEffect.transform.SetParent(board.transform); 
        Destroy(damageEffect, 2f); 
    }
}
