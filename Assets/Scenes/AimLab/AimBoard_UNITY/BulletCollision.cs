using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeaponButton")) // Butona temas etti mi?
        {
            string buttonName = other.name; // Çarpýlan butonun adý

            // WeaponSelector script'ine eriþ
            WeaponSelector weaponSelector = FindObjectOfType<WeaponSelector>();
            if (weaponSelector != null)
            {
                weaponSelector.SelectWeapon(buttonName);
            }

            // Mermiyi yok et
            Destroy(gameObject);
        }
    }
}
