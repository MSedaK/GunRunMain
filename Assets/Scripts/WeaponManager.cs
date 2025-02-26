using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject weaponA;
    public GameObject weaponB;
    public GameObject weaponC;
    public GameObject weaponD;

    [Header("Weapon UI Images")]
    public Image weaponAImage;
    public Image weaponBImage;
    public Image weaponCImage;
    public Image weaponDImage;

    [Header("Weapon VFX")]
    public GameObject vfxA;
    public GameObject vfxB;
    public GameObject vfxC;
    public GameObject vfxD;

    private int currentWeapon = 0;
    private int enemyKillCount = 0;

    [Header("Weapon Switch Settings")]
    public int killsToWeaponB = 10;
    public int killsToWeaponC = 22;
    public int killsToWeaponD = 36;
    public float vfxDelay = 0.5f;

    private void Start()
    {
        UpdateWeaponUI(); 
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled()
    {
        enemyKillCount++;
        CheckWeaponSwitch(enemyKillCount);
    }

    public void CheckWeaponSwitch(int killCount)
    {
        if (currentWeapon == 0 && killCount >= killsToWeaponB)
        {
            StartCoroutine(SwitchWeaponWithVFX(weaponA, weaponB, vfxA, vfxB)); // Eski ve yeni silah VFX'lerini geçiyoruz
            currentWeapon = 1;
        }
        else if (currentWeapon == 1 && killCount >= killsToWeaponC)
        {
            StartCoroutine(SwitchWeaponWithVFX(weaponB, weaponC, vfxB, vfxC)); // Eski ve yeni silah VFX'lerini geçiyoruz
            currentWeapon = 2;
        }

        UpdateWeaponUI();
    }

    private IEnumerator SwitchWeaponWithVFX(GameObject currentWeaponObj, GameObject nextWeaponObj, GameObject currentWeaponVFX, GameObject nextWeaponVFX)
    {
        // 1. Eski silahýn VFX'ini çalýþtýr ve 3 saniye sonra yok et
        if (currentWeaponVFX != null)
        {
            GameObject spawnedVFX = Instantiate(currentWeaponVFX, currentWeaponObj.transform.position, Quaternion.identity);
            Destroy(spawnedVFX, 3f); // VFX 3 saniye sonra yok olur
        }

        currentWeaponObj.SetActive(false);

        yield return new WaitForSeconds(vfxDelay);

        // 4. Yeni silahýn VFX'ini çalýþtýr ve 3 saniye sonra yok et
        if (nextWeaponVFX != null)
        {
            GameObject spawnedVFX = Instantiate(nextWeaponVFX, nextWeaponObj.transform.position, Quaternion.identity);
            Destroy(spawnedVFX, 3f); // VFX 3 saniye sonra yok olur
        }

        // 5. Yeni silahý aç
        nextWeaponObj.SetActive(true);

        // Silah deðiþtirildiðini log'a yaz
        Debug.Log("Yeni silaha geçildi: " + nextWeaponObj.name);

        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        SetWeaponUIImageAlpha(weaponAImage, currentWeapon == 0 ? 1f : 0.5f);
        SetWeaponUIImageAlpha(weaponBImage, currentWeapon == 1 ? 1f : 0.5f);
        SetWeaponUIImageAlpha(weaponCImage, currentWeapon == 2 ? 1f : 0.5f);
    }

    private void SetWeaponUIImageAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
