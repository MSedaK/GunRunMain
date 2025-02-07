using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class GunFire : MonoBehaviour
{
    public float velocity;
    public GameObject bulletPrefab;
    public Transform barrel;
    public Transform targetDirection;
    public AudioSource audioSource;
    public ParticleSystem ps;
    public Animator gunAnimator;
    public GameObject muzzleFlashPrefab;

    [Header("Haptic Feedback Settings")]
    public float hapticStrength = 0.5f;

    [Header("Bullet Settings")]
    public AudioClip bulletHitSound;  
    public GameObject damageEffectPrefab;  

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Fire();
            StartCoroutine(HapticFeedback());
        }
    }

    public void Fire()
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, barrel.position, Quaternion.LookRotation(targetDirection.position - barrel.position));

        spawnedBullet.GetComponent<Rigidbody>().velocity = velocity * (targetDirection.position - barrel.position).normalized;

        Bullet bulletScript = spawnedBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.hitSound = bulletHitSound;
            bulletScript.damageEffectPrefab = damageEffectPrefab;
        }

        audioSource.Play();

        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Shoot");
        }

        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, barrel.position, barrel.rotation);
            Destroy(flash, 0.2f);
        }

        if (ps != null)
        {
            ps.Play();
        }

        Destroy(spawnedBullet, 2f);
    }

    private IEnumerator HapticFeedback()
    {
        OVRInput.SetControllerVibration(1, hapticStrength, OVRInput.Controller.RTouch);
        yield return new WaitForSeconds(0.1f);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
