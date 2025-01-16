using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public GameObject portalPrefabA; 
    public GameObject portalPrefabB; 
    public GameObject portalPrefabC; 
    public GameObject enemyPrefab;

    void Start()
    {
        SpawnPortal(portalPrefabA, new Vector3(6.7f, 18.1f, -30.1f)); 
        SpawnPortal(portalPrefabB, new Vector3(-2f, 15.9f, 30.1f)); 
        SpawnPortal(portalPrefabC, new Vector3(-76.5f, 13.3f, -12.2f)); 
    }

    void SpawnPortal(GameObject portalPrefab, Vector3 position)
    {
        GameObject portal = Instantiate(portalPrefab, position, Quaternion.identity);

        StartCoroutine(SpawnEnemyWithDelay(portal.transform.position));
    }

    IEnumerator SpawnEnemyWithDelay(Vector3 portalPosition)
    {
        yield return new WaitForSeconds(1.5f);

        Instantiate(enemyPrefab, portalPosition, Quaternion.identity);
    }
}

