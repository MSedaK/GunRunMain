using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GunFire : MonoBehaviour
{
    public float velocity;
    public GameObject bulletPrefab;
    public Transform barrel;
    public AudioSource audioSource;
    public ParticleSystem ps;

    [Header("Haptic Feedback Settings")]
    public float hapticStrength = 0.5f; 

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
        GameObject spawnedBullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = velocity * barrel.forward;

        audioSource.Play();

        Animator anim;
        if (TryGetComponent<Animator>(out anim))
        {
            anim.SetTrigger("Fire");
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
