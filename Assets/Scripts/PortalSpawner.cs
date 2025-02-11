using System.Collections;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public GameObject portalPrefabA;
    public GameObject portalPrefabB;
    public GameObject portalPrefabC;

    [Header("Enemy Prefabs for Portals")]
    public GameObject enemyType1A;
    public GameObject enemyType2A;
    public GameObject enemyType1B;
    public GameObject enemyType2B;
    public GameObject enemyType1C;
    public GameObject enemyType2C;

    [Header("Enemy Spawn Settings")]
    public float enemySpawnInterval = 2f;
    public float delayBeforeEnemySpawn = 10f; 

    [Header("Portal Spawn Points")]
    public Vector3[] spawnPointsA;
    public Vector3[] spawnPointsB;
    public Vector3[] spawnPointsC;

    private int enemySpawnIndexA = 0;
    private int enemySpawnIndexB = 0;
    private int enemySpawnIndexC = 0;

    void Start()
    {
        StartCoroutine(SpawnPortals()); 
    }

    IEnumerator SpawnPortals()
    {
        SpawnPortalWithRotation(portalPrefabA, new Vector3(5.2f, -4.3f, -87.1f), Quaternion.Euler(0, 90, 0));
        SpawnPortalWithRotation(portalPrefabB, new Vector3(5.2f, -4.5f, 105f), Quaternion.Euler(0, 90, 0));
        SpawnPortal(portalPrefabC, new Vector3(-116.2f, -4.6f, 1.8f));

        yield return new WaitForSeconds(delayBeforeEnemySpawn); 
        StartCoroutine(SpawnEnemyFromPointsA());
        StartCoroutine(SpawnEnemyFromPointsB());
        StartCoroutine(SpawnEnemyFromPointsC());
    }

    void SpawnPortal(GameObject portalPrefab, Vector3 position)
    {
        Instantiate(portalPrefab, position, Quaternion.identity);
    }

    void SpawnPortalWithRotation(GameObject portalPrefab, Vector3 position, Quaternion rotation)
    {
        Instantiate(portalPrefab, position, rotation);
    }

    IEnumerator SpawnEnemyFromPointsA()
    {
        while (true)
        {
            if (spawnPointsA.Length == 0) yield break;

            Vector3 spawnPos = spawnPointsA[enemySpawnIndexA];
            GameObject enemyPrefab = (enemySpawnIndexA % 2 == 0) ? enemyType1A : enemyType2A;
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            enemySpawnIndexA = (enemySpawnIndexA + 1) % spawnPointsA.Length;
            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    IEnumerator SpawnEnemyFromPointsB()
    {
        while (true)
        {
            if (spawnPointsB.Length == 0) yield break;

            Vector3 spawnPos = spawnPointsB[enemySpawnIndexB];
            GameObject enemyPrefab = (enemySpawnIndexB % 2 == 0) ? enemyType1B : enemyType2B;
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            enemySpawnIndexB = (enemySpawnIndexB + 1) % spawnPointsB.Length;
            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    IEnumerator SpawnEnemyFromPointsC()
    {
        while (true)
        {
            if (spawnPointsC.Length == 0) yield break;

            Vector3 spawnPos = spawnPointsC[enemySpawnIndexC];
            GameObject enemyPrefab = (enemySpawnIndexC % 2 == 0) ? enemyType1C : enemyType2C;
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            enemySpawnIndexC = (enemySpawnIndexC + 1) % spawnPointsC.Length;
            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }
}
