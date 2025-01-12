using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject weaponA;
    public GameObject weaponB;
    public GameObject weaponC;

    private int rightIndexPressCount = 0; // Sað index tuþuna basýlma sayýsý
    private int currentWeapon = 0;

    public int pressesToWeaponB = 5; // B'ye geçmek için gereken sað index tuþu sayýsý
    public int pressesToWeaponC = 10; // C'ye geçmek için gereken sað index tuþu sayýsý

    private int shootCount = 0;

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
            SwitchWeapon(weaponA, weaponB);
            currentWeapon = 1;
        }
        else if (currentWeapon == 1 && shootCount >= pressesToWeaponC)
        {
            SwitchWeapon(weaponB, weaponC);
            currentWeapon = 2;
        }
    }

    private void SwitchWeapon(GameObject currentWeaponObj, GameObject nextWeaponObj)
    {
        currentWeaponObj.SetActive(false);
        nextWeaponObj.SetActive(true);

        Debug.Log("Yeni silaha geçildi: " + nextWeaponObj.name);
    }
}
