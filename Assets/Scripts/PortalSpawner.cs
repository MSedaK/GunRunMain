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
            EnemyWaveData waveData = GetWaveData(currentWave);
            yield return StartCoroutine(SpawnWave(waveData, enemySpeed));
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

        for (int i = 0; i < waveData.enemyCount; i++)
        {
            yield return SpawnEnemyAtPortal(spawnPointsA, waveData.GetEnemyForPortal("A", i), enemySpeed);
            yield return SpawnEnemyAtPortal(spawnPointsB, waveData.GetEnemyForPortal("B", i), enemySpeed);
            yield return SpawnEnemyAtPortal(spawnPointsC, waveData.GetEnemyForPortal("C", i), enemySpeed);
        }
    }

    IEnumerator SpawnEnemyAtPortal(Vector3[] spawnPoints, GameObject enemyType, float enemySpeed)
    {
        if (!isSpawning || enemyType == null) yield break;

        Vector3 spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyType, spawnPos, Quaternion.identity);

        EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
        if (enemyBehavior != null)
        {
            enemyBehavior.speed = enemySpeed;
        }

        yield return new WaitForSeconds(enemySpawnInterval);
    }

    float GetEnemySpeedForWave(int waveNumber)
    {
        return 3f + (waveNumber - 1) * 2f;
    }

    EnemyWaveData GetWaveData(int wave)
    {
        switch (wave)
        {
            case 1:
                return new EnemyWaveData(new GameObject[] { jamEnemy, jamEnemy, flyingEnemy }, 3);
            case 2:
                return new EnemyWaveData(new GameObject[] { flyingEnemy, flyingEnemy, tallEnemy }, 3);
            case 3:
                return new EnemyWaveData(new GameObject[] { tallEnemy, tallEnemy, middleEnemy }, 3);
            case 4:
                return new EnemyWaveData(new GameObject[] { middleEnemy, jamEnemy, jamEnemy }, 3);
            default:
                return new EnemyWaveData(new GameObject[] { jamEnemy, flyingEnemy, tallEnemy }, 3);
        }
    }
}

public class EnemyWaveData
{
    private GameObject[] portalEnemies;
    public int enemyCount;

    public EnemyWaveData(GameObject[] enemies, int count)
    {
        portalEnemies = enemies;
        enemyCount = count;
    }

    public GameObject GetEnemyForPortal(string portal, int index)
    {
        if (portal == "A") return portalEnemies[0];
        if (portal == "B") return portalEnemies[1];
        if (portal == "C") return portalEnemies[2];
        return null;
    }
}
