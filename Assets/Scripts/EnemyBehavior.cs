using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed = 1;

    private void Start()
    {
        
    }

    private void Update()
    {
        Vector3 targetPosition = Camera.main.transform.position;    

        agent.SetDestination(targetPosition);
        agent.speed = speed;
    }
}