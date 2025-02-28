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

    [Header("Weapon Scale Settings")]
    public float activeGlobalScale = 1.2f;
    public float inactiveGlobalScale = 1.0f;

    [Header("UI Image Scale Settings")]
    public float activeUIImageScale = 0.8f;
    public float inactiveUIImageScale = 0.6f;

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
            StartCoroutine(SwitchWeaponWithVFX(weaponA, weaponB, vfxA, vfxB));
            currentWeapon = 1;
            HandleBarettaSwitch(weaponB);
        }
        else if (currentWeapon == 1 && killCount >= killsToWeaponC)
        {
            StartCoroutine(SwitchWeaponWithVFX(weaponB, weaponC, vfxB, vfxC));
            currentWeapon = 2;
            HandleBarettaSwitch(weaponC);
        }
        else if (currentWeapon == 2 && killCount >= killsToWeaponD)
        {
            StartCoroutine(SwitchWeaponWithVFX(weaponC, weaponD, vfxC, vfxD));
            currentWeapon = 3;
            HandleBarettaSwitch(weaponD);
        }

        UpdateWeaponUI();
    }

    private void HandleBarettaSwitch(GameObject newWeapon)
    {
        GunFire gunFire = newWeapon.GetComponent<GunFire>();
        if (gunFire != null && gunFire.isBaretta)
        {
            gunFire.isLeftHanded = false; 
            if (!gunFire.isAutomatic)
            {
                gunFire.enabled = false;
            }
        }
    }

    private IEnumerator SwitchWeaponWithVFX(GameObject currentWeaponObj, GameObject nextWeaponObj, GameObject currentWeaponVFX, GameObject nextWeaponVFX)
    {
        if (currentWeaponVFX != null)
        {
            GameObject spawnedVFX = Instantiate(currentWeaponVFX, currentWeaponObj.transform.position, Quaternion.identity);
            Destroy(spawnedVFX, 1f);
        }

        currentWeaponObj.SetActive(false);

        yield return new WaitForSeconds(vfxDelay);

        if (nextWeaponVFX != null)
        {
            GameObject spawnedVFX = Instantiate(nextWeaponVFX, nextWeaponObj.transform.position, Quaternion.identity);
            Destroy(spawnedVFX, 1f);
        }

        nextWeaponObj.SetActive(true);

        Debug.Log("Yeni silaha geçildi: " + nextWeaponObj.name);

        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        SetWeaponUIImageAlphaAndScale(weaponAImage, currentWeapon == 0);
        SetWeaponUIImageAlphaAndScale(weaponBImage, currentWeapon == 1);
        SetWeaponUIImageAlphaAndScale(weaponCImage, currentWeapon == 2);
        SetWeaponUIImageAlphaAndScale(weaponDImage, currentWeapon == 3);
    }

    private void SetWeaponUIImageAlphaAndScale(Image image, bool isActive)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = isActive ? 1f : 0.5f;
            image.color = color;

            image.rectTransform.localScale = Vector3.one * (isActive ? activeUIImageScale : inactiveUIImageScale);
        }
    }

}