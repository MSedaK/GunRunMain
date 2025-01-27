using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public GameObject portalPrefabA; // Portal A prefab
    public GameObject portalPrefabB; // Portal B prefab
    public GameObject portalPrefabC; // Portal C prefab

    public GameObject enemyPrefabA; // Portal A'nýn düþmaný
    public GameObject enemyPrefabB; // Portal B'nin düþmaný
    public GameObject enemyPrefabC; // Portal C'nin düþmaný

    public float enemySpawnInterval = 2f; // Her düþman spawn arasý süre (2 saniye)

    // Her portal için spawn noktalarý
    public Vector3 spawnPointA = new Vector3(0.3f, 1.5f, 0.0f); // Portal A'nýn spawn noktasý
    public Vector3 spawnPointB = new Vector3(0.3f, 1.6f, 0.0f); // Portal B'nin spawn noktasý
    public Vector3 spawnPointC = new Vector3(-115.35f, 0.34661f, 1.7658f); // Portal C'nin spawn noktasý

    void Start()
    {
        // Portallarý oluþtur
        SpawnPortalWithRotation(portalPrefabA, new Vector3(5.2f, -4.3f, -87.1f), Quaternion.Euler(0, -90, 0));
        SpawnPortalWithRotation(portalPrefabB, new Vector3(5.2f, -4.5f, 105f), Quaternion.Euler(0, 90, 0));
        SpawnPortal(portalPrefabC, new Vector3(-116.2f, -4.6f, 1.8f));

        // Belirtilen spawn noktalarýndan düþman spawn etmeye baþla
        StartCoroutine(SpawnEnemyFromPoint(spawnPointA, enemyPrefabA, Quaternion.Euler(0, 90, 0))); // Portal A'dan çýkan düþman 90 derece döndürülüyor
        StartCoroutine(SpawnEnemyFromPoint(spawnPointB, enemyPrefabB, Quaternion.Euler(0, -90, 0))); // Portal B'den çýkan düþman -90 derece döndürülüyor
        StartCoroutine(SpawnEnemyFromPoint(spawnPointC, enemyPrefabC, Quaternion.Euler(0, 180, 0))); // Portal C'den çýkan düþman 180 derece döndürülüyor
    }

    void SpawnPortal(GameObject portalPrefab, Vector3 position)
    {
        // Portalý belirtilen pozisyonda oluþtur
        Instantiate(portalPrefab, position, Quaternion.identity);
    }

    void SpawnPortalWithRotation(GameObject portalPrefab, Vector3 position, Quaternion rotation)
    {
        // Portalý belirtilen pozisyon ve rotasyonla oluþtur
        Instantiate(portalPrefab, position, rotation);
    }

    IEnumerator SpawnEnemyFromPoint(Vector3 spawnPosition, GameObject enemyPrefab, Quaternion rotation)
    {
        while (true)
        {
            // Belirtilen pozisyonda ve rotasyonla düþman spawn et
            Instantiate(enemyPrefab, spawnPosition, rotation);

            // 2 saniye bekle
            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }
}
