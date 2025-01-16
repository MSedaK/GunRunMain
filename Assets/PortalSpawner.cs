using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public GameObject portalPrefab; 
    public GameObject enemyPrefab; 
    void Start()
    {
        SpawnPortal(new Vector3(6.7f, 18.1f, -30.1f)); 
        SpawnPortal(new Vector3(-2f, 15.9f, 30.1f)); 
        SpawnPortal(new Vector3(-10.68f, 17f, -1.86f)); 
    }

    void SpawnPortal(Vector3 position)
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
