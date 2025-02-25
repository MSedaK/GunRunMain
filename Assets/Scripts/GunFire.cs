using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using Meta.XR.MRUtilityKit;

public class GunFire : MonoBehaviour
{
    public float velocity;
    public GameObject bulletPrefab;

    public Transform barrel1;
    public Transform barrel2; 
    public Transform targetDirection1;
    public Transform targetDirection2; 

    public AudioSource audioSource;
    public ParticleSystem ps;
    public Animator gunAnimator;
    public GameObject muzzleFlashPrefab;

    [Header("Haptic Feedback Settings")]
    public float hapticStrength = 0.5f;

    [Header("Bullet Settings")]
    public AudioClip bulletHitSound;
    public GameObject damageEffectPrefab;

    [Header("Ammo Settings")]
    public int maxAmmo = 20;
    private int currentAmmo;
    public TextMeshProUGUI ammoText;
    public GameObject ammoUI;

    [Header("Weapon Settings")]
    public bool useDualBarrel = false;

    [Header("Fire Settings")]
    public float fireCooldown = 0.5f; 
    private bool canFire = true; 
    private Coroutine fireCooldownCoroutine; 


    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();

        EnemyHealth.OnEnemyKilled += Reload;
    }

    void OnDestroy()
    {
        EnemyHealth.OnEnemyKilled -= Reload;
    }

    void Update()
    {
        if (ammoUI != null)
        {
            ammoUI.transform.rotation = Quaternion.LookRotation(ammoUI.transform.position - Camera.main.transform.position);
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && currentAmmo > 0)
        {
            Fire();
            StartCoroutine(HapticFeedback());
            currentAmmo -= useDualBarrel ? 2 : 1; 
            UpdateAmmoDisplay();
            canFire = false;
            fireCooldownCoroutine = StartCoroutine(FireCooldown());
        }
        else if (currentAmmo <= 0)
        {
            Reload();
        }
    }

    private IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }

    public void Fire()
    {
        FireFromBarrel(barrel1, targetDirection1);

        if (useDualBarrel && barrel2 != null && targetDirection2 != null)
        {
            FireFromBarrel(barrel2, targetDirection2);
        }

        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Shoot");
        }

        if (ps != null)
        {
            ps.Play();
        }
    }

    private void FireFromBarrel(Transform barrel, Transform target)
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, barrel.position, Quaternion.LookRotation(target.position - barrel.position));
        spawnedBullet.GetComponent<Rigidbody>().velocity = velocity * (target.position - barrel.position).normalized;

        Bullet bulletScript = spawnedBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.hitSound = bulletHitSound;
            bulletScript.damageEffectPrefab = damageEffectPrefab;
        }

        audioSource.Play();

        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, barrel.position, barrel.rotation);
            Destroy(flash, 0.2f);
        }

        Destroy(spawnedBullet, 2f);
    }

    private IEnumerator HapticFeedback()
    {
        OVRInput.SetControllerVibration(1, hapticStrength, OVRInput.Controller.RTouch);
        yield return new WaitForSeconds(0.1f);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();
    }

    public void UpdateAmmoDisplay()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }
    }
}
