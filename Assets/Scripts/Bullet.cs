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
            else if (other.CompareTag("TargetBoard"))
            {
                TargetBoardHealth boardHealth = other.GetComponent<TargetBoardHealth>();
                if (boardHealth != null)
                {
                    boardHealth.TakeDamage(damage, other.ClosestPoint(transform.position));
                }
            }

            Destroy(gameObject);
        }
    }
}
