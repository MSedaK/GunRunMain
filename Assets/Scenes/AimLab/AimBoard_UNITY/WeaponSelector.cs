using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public GameObject Canik; // 1. Silah
    public GameObject Silencer; // 2. Silah
    public GameObject Shotgun; // 3. Silah

    private GameObject activeWeapon; // Þu anda aktif olan silah

    void Start()
    {
        // Baþlangýç durumunda Canik aktif, diðerleri kapalý
        activeWeapon = Canik;
        Canik.SetActive(true);
        Silencer.SetActive(false);
        Shotgun.SetActive(false);
    }

    public void SelectWeapon(GameObject newWeapon)
    {
        if (activeWeapon != null)
        {
            // Mevcut aktif silahý devre dýþý býrak
            activeWeapon.SetActive(false);
        }

        // Yeni silahý aktif et
        activeWeapon = newWeapon;
        activeWeapon.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Ateþ etme tuþu (örneðin sol fare týký)
        {
            // Namludan ray çýkar
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // VR/XR için baþka bir ray çýkýþ noktasý olabilir.
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("WeaponButton")) // Butona özel "WeaponButton" tag'ý kontrol et
                {
                    // Çarptýðý butonun adýný al
                    string buttonName = hit.collider.name;

                    // Buton adýna göre silah seçimini yap
                    if (buttonName.Contains("Canik"))
                    {
                        SelectWeapon(Canik);
                    }
                    else if (buttonName.Contains("Silencer"))
                    {
                        SelectWeapon(Silencer);
                    }
                    else if (buttonName.Contains("Shotgun"))
                    {
                        SelectWeapon(Shotgun);
                    }
                }
            }
        }
    }
}
