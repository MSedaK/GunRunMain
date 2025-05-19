using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdvancedPortalSpawner : MonoBehaviour
{
    [Header("Portal Prefabs")]
    public GameObject portalPrefabA, portalPrefabB, portalPrefabC;

    [Header("Enemy Prefabs")]
    public GameObject tur1Enemy; 
    public GameObject tur2Enemy;  
    public GameObject tur3Enemy; 

    [Header("Spawn Settings")]
    public float enemySpawnInterval = 2f;
    public float delayBeforeFirstSpawn = 10f;
    public int enemiesPerStage = 30;

    [Header("Spawn Points")]
    public Vector3[] spawnPointsA, spawnPointsB, spawnPointsC;

    private List<string> portals = new List<string> { "A", "B", "C" };
    private Dictionary<string, Vector3[]> portalSpawnPoints;
    private Dictionary<string, GameObject> portalPrefabs;

    private Dictionary<string, int> consecutivePortalCounts = new();
    private Dictionary<string, GameObject> lastEnemyPerPortal = new();

    private bool isGameOver = false;
    private bool isSpawning = false;

    void Start()
    {
        portalSpawnPoints = new()
        {
            { "A", spawnPointsA },
            { "B", spawnPointsB },
            { "C", spawnPointsC }
        };

        portalPrefabs = new()
        {
            { "A", portalPrefabA },
            { "B", portalPrefabB },
            { "C", portalPrefabC }
        };

        SpawnPortals();
        StartCoroutine(StartStageSpawning());
    }

    void SpawnPortals()
    {
        Instantiate(portalPrefabA, new Vector3(5.2f, -4.3f, -87.1f), Quaternion.Euler(0, 90, 0));
        Instantiate(portalPrefabB, new Vector3(5.2f, -4.5f, 105f), Quaternion.Euler(0, 90, 0));
        Instantiate(portalPrefabC, new Vector3(-116.2f, -4.6f, 1.8f), Quaternion.identity);
    }

    IEnumerator StartStageSpawning()
    {
        yield return new WaitForSeconds(delayBeforeFirstSpawn);

        int stage = 1;
        while (!isGameOver)
        {
            isSpawning = true;
            List<GameObject> enemyList = GetEnemiesByStage(stage);
            yield return StartCoroutine(SpawnEnemies(enemyList));
            isSpawning = false;

            stage++;
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnEnemies(List<GameObject> enemies)
    {
        if (!isSpawning || isGameOver) yield break;

        consecutivePortalCounts.Clear();
        lastEnemyPerPortal.Clear();

        for (int i = 0; i < enemies.Count; i++)
        {
            if (!isSpawning || isGameOver) yield break;

            GameObject selectedEnemy = enemies[i];
            string selectedPortal = GetValidPortalForEnemy(selectedEnemy);

            if (selectedPortal == null)
            {
                yield return new WaitForSeconds(0.5f);
                i--;
                continue;
            }

            Vector3 spawnPos = GetRandomSpawnPoint(selectedPortal);
            GameObject enemy = Instantiate(selectedEnemy, spawnPos, Quaternion.identity);

            enemy.tag = "Enemy";

            if (!consecutivePortalCounts.ContainsKey(selectedPortal))
                consecutivePortalCounts[selectedPortal] = 0;

            consecutivePortalCounts[selectedPortal]++;
            foreach (var portal in portals.Where(p => p != selectedPortal))
                consecutivePortalCounts[portal] = 0;

            lastEnemyPerPortal[selectedPortal] = selectedEnemy;

            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    string GetValidPortalForEnemy(GameObject enemy)
    {
        List<string> validPortals = new();

        foreach (string portal in portals)
        {
            if (consecutivePortalCounts.TryGetValue(portal, out int count) && count >= 2)
                continue;

            if (lastEnemyPerPortal.TryGetValue(portal, out GameObject lastEnemy) && lastEnemy == enemy)
                continue;

            validPortals.Add(portal);
        }

        var currentSameTypeCount = lastEnemyPerPortal.Values.Count(v => v == enemy);
        if (currentSameTypeCount >= 2)
        {
            validPortals = validPortals.Where(p => lastEnemyPerPortal.TryGetValue(p, out var e) && e != enemy).ToList();
        }

        if (validPortals.Count == 0) return null;
        return validPortals[Random.Range(0, validPortals.Count)];
    }

    Vector3 GetRandomSpawnPoint(string portal)
    {
        Vector3[] points = portalSpawnPoints[portal];
        return points[Random.Range(0, points.Length)];
    }

    List<GameObject> GetEnemiesByStage(int stage)
    {
        Dictionary<GameObject, float> spawnRatios = stage switch
        {
            1 => new() { { tur1Enemy, 0.5f }, { tur2Enemy, 0.5f } },
            2 => new() { { tur1Enemy, 0.45f }, { tur2Enemy, 0.45f }, { tur3Enemy, 0.1f } },
            3 => new() { { tur1Enemy, 0.3f }, { tur2Enemy, 0.3f }, { tur3Enemy, 0.4f } },
            _ => new() { { tur1Enemy, 0.3f }, { tur2Enemy, 0.3f }, { tur3Enemy, 0.4f } },
        };

        List<GameObject> result = new();
        foreach (var kvp in spawnRatios)
        {
            int count = Mathf.RoundToInt(kvp.Value * enemiesPerStage);
            for (int i = 0; i < count; i++)
                result.Add(kvp.Key);
        }

        for (int i = 0; i < result.Count; i++)
        {
            int rnd = Random.Range(i, result.Count);
            (result[i], result[rnd]) = (result[rnd], result[i]);
        }

        return result;
    }

    public void StopSpawning()
    {
        isGameOver = true;
        isSpawning = false;
        StopAllCoroutines();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
