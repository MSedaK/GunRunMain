using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public GameObject Canik;
    public GameObject Silencer;
    public GameObject Shotgun;

    private GameObject activeWeapon;

    void Start()
    {
        activeWeapon = Canik;
        Canik.SetActive(true);
        Silencer.SetActive(false);
        Shotgun.SetActive(false);
    }

    public void SelectWeapon(string weaponName)
    {
        if (activeWeapon != null)
        {
            activeWeapon.SetActive(false);
        }

        if (weaponName.Contains("Canik"))
        {
            activeWeapon = Canik;
        }
        else if (weaponName.Contains("Silencer"))
        {
            activeWeapon = Silencer;
        }
        else if (weaponName.Contains("Shotgun"))
        {
            activeWeapon = Shotgun;
        }

        activeWeapon.SetActive(true);
    }
}
