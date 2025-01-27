using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject weaponA;
    public GameObject weaponB;
    public GameObject weaponC;

    [Header("Weapon VFX")]
    public GameObject vfxA; // Silah A için VFX prefab
    public GameObject vfxB; // Silah B için VFX prefab
    public GameObject vfxC; // Silah C için VFX prefab

    private int currentWeapon = 0; // Aktif silahýn indeksi
    private int shootCount = 0; // Ateþ sayýsý

    [Header("Weapon Switch Settings")]
    public int pressesToWeaponB = 5; // B'ye geçmek için gereken ateþ sayýsý
    public int pressesToWeaponC = 10; // C'ye geçmek için gereken ateþ sayýsý
    public float vfxDelay = 0.5f; // VFX'in ardýndan silahýn görünmesi için geçen süre

    private void Update()
    {
        // Sað index tuþuna basýlma kontrolü
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            shootCount++;
            CheckWeaponSwitch(shootCount);
        }
    }

    public void CheckWeaponSwitch(int shootCount)
    {
        if (currentWeapon == 0 && shootCount >= pressesToWeaponB)
        {
            StartCoroutine(SwitchWeaponWithVFX(weaponA, weaponB, vfxA));
            currentWeapon = 1;
        }
        else if (currentWeapon == 1 && shootCount >= pressesToWeaponC)
        {
            StartCoroutine(SwitchWeaponWithVFX(weaponB, weaponC, vfxB));
            currentWeapon = 2;
        }
    }

    private IEnumerator SwitchWeaponWithVFX(GameObject currentWeaponObj, GameObject nextWeaponObj, GameObject vfx)
    {
        // Mevcut silahý gizle
        currentWeaponObj.SetActive(false);

        // VFX'i çalýþtýr
        if (vfx != null)
        {
            GameObject spawnedVFX = Instantiate(vfx, currentWeaponObj.transform.position, currentWeaponObj.transform.rotation);
            Destroy(spawnedVFX, 2f); // VFX'i 2 saniye sonra yok et
        }

        // VFX süresince bekle
        yield return new WaitForSeconds(vfxDelay);

        // Yeni silahý aktif hale getir
        nextWeaponObj.SetActive(true);

        Debug.Log("Yeni silaha geçildi: " + nextWeaponObj.name);
    }
}
