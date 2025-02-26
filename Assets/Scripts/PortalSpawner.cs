using System.Collections;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public GameObject portalPrefabA;
    public GameObject portalPrefabB;
    public GameObject portalPrefabC;

    [Header("Enemy Prefabs for Portals")]
    public GameObject jamEnemy;
    public GameObject flyingEnemy;
    public GameObject tallEnemy;
    public GameObject middleEnemy;

    [Header("Enemy Spawn Settings")]
    public float enemySpawnInterval = 1.5f;
    public float delayBeforeFirstWave = 10f;
    public float waveDelay = 5f;
    public float postWaveSpawnDelay = 5f;

    [Header("Portal Spawn Points")]
    public Vector3[] spawnPointsA;
    public Vector3[] spawnPointsB;
    public Vector3[] spawnPointsC;

    private int currentWave = 1;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnPortals());
    }

    IEnumerator SpawnPortals()
    {
        SpawnPortalWithRotation(portalPrefabA, new Vector3(5.2f, -4.3f, -87.1f), Quaternion.Euler(0, 90, 0));
        SpawnPortalWithRotation(portalPrefabB, new Vector3(5.2f, -4.5f, 105f), Quaternion.Euler(0, 90, 0));
        SpawnPortal(portalPrefabC, new Vector3(-116.2f, -4.6f, 1.8f));

        yield return new WaitForSeconds(delayBeforeFirstWave);

        while (true)
        {
            isSpawning = true;
            float enemySpeed = GetEnemySpeedForWave(currentWave);
            yield return StartCoroutine(SpawnWave(new EnemyWaveData(1, jamEnemy, 1, flyingEnemy, 1, middleEnemy, 1, tallEnemy, 1, flyingEnemy, 1, middleEnemy), enemySpeed));
            isSpawning = false;
            yield return new WaitForSeconds(postWaveSpawnDelay);
            currentWave++;
        }
    }

    void SpawnPortal(GameObject portalPrefab, Vector3 position)
    {
        Instantiate(portalPrefab, position, Quaternion.identity);
    }

    void SpawnPortalWithRotation(GameObject portalPrefab, Vector3 position, Quaternion rotation)
    {
        Instantiate(portalPrefab, position, rotation);
    }

    IEnumerator SpawnWave(EnemyWaveData waveData, float enemySpeed)
    {
        if (!isSpawning) yield break;

        yield return StartCoroutine(SpawnEnemiesAtPortal(spawnPointsA, waveData.A1, waveData.enemyA1, enemySpeed));
        yield return StartCoroutine(SpawnEnemiesAtPortal(spawnPointsB, waveData.B1, waveData.enemyB1, enemySpeed));
        yield return StartCoroutine(SpawnEnemiesAtPortal(spawnPointsC, waveData.C1, waveData.enemyC1, enemySpeed));

        yield return StartCoroutine(SpawnEnemiesAtPortal(spawnPointsA, waveData.A2, waveData.enemyA2, enemySpeed));
        yield return StartCoroutine(SpawnEnemiesAtPortal(spawnPointsB, waveData.B2, waveData.enemyB2, enemySpeed));
        yield return StartCoroutine(SpawnEnemiesAtPortal(spawnPointsC, waveData.C2, waveData.enemyC2, enemySpeed));
    }

    IEnumerator SpawnEnemiesAtPortal(Vector3[] spawnPoints, int enemyCount, GameObject enemyType, float enemySpeed)
    {
        if (!isSpawning) yield break;

        for (int i = 0; i < enemyCount; i++)
        {
            if (!isSpawning) yield break;
            if (spawnPoints.Length == 0 || enemyType == null) yield break;

            Vector3 spawnPos = spawnPoints[i % spawnPoints.Length];
            GameObject enemy = Instantiate(enemyType, spawnPos, Quaternion.identity);

            EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
            if (enemyBehavior != null)
            {
                enemyBehavior.speed = enemySpeed;
            }

            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    float GetEnemySpeedForWave(int waveNumber)
    {
        return 3f + (waveNumber - 1) * 2f;
    }
}

public class EnemyWaveData
{
    public int A1, B1, C1, A2, B2, C2;
    public GameObject enemyA1, enemyB1, enemyC1, enemyA2, enemyB2, enemyC2;

    public EnemyWaveData(
        int a1, GameObject eA1, int b1, GameObject eB1, int c1, GameObject eC1,
        int a2 = 0, GameObject eA2 = null, int b2 = 0, GameObject eB2 = null, int c2 = 0, GameObject eC2 = null)
    {
        A1 = a1; enemyA1 = eA1;
        B1 = b1; enemyB1 = eB1;
        C1 = c1; enemyC1 = eC1;
        A2 = a2; enemyA2 = eA2;
        B2 = b2; enemyB2 = eB2;
        C2 = c2; enemyC2 = eC2;
    }
}
