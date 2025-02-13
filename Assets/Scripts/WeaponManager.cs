using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject weaponA;
    public GameObject weaponB;
    public GameObject weaponC;

    [Header("Weapon UI Images")]
    public Image weaponAImage;
    public Image weaponBImage;
    public Image weaponCImage;

    [Header("Weapon VFX")]
    public GameObject vfxA;
    public GameObject vfxB;
    public GameObject vfxC;

    private int currentWeapon = 0;
    private int enemyKillCount = 0;

    [Header("Weapon Switch Settings")]
    public int killsToWeaponB = 6;
    public int killsToWeaponC = 8;
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
            StartCoroutine(SwitchWeaponWithVFX(weaponA, weaponB, vfxA));
            currentWeapon = 1;
            UpdateWeaponUI();
        }
        else if (currentWeapon == 1 && killCount >= killsToWeaponC)
        {
            StartCoroutine(SwitchWeaponWithVFX(weaponB, weaponC, vfxB));
            currentWeapon = 2;
            UpdateWeaponUI();
        }
    }

    private IEnumerator SwitchWeaponWithVFX(GameObject currentWeaponObj, GameObject nextWeaponObj, GameObject vfx)
    {
        currentWeaponObj.SetActive(false);

        if (vfx != null)
        {
            GameObject spawnedVFX = Instantiate(vfx, currentWeaponObj.transform.position, currentWeaponObj.transform.rotation);
            Destroy(spawnedVFX, 2f);
        }

        yield return new WaitForSeconds(vfxDelay);

        nextWeaponObj.SetActive(true);
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
