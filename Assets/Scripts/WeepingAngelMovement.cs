using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeepingAngelMovement : MonoBehaviour
{
    [SerializeField] private float playerDetectionRadius = 5f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float freezeDuration = 2f;
    [SerializeField] private LayerMask weepingAngelLayer;

    private GameObject player;
    private NavMeshAgent agent;
    private float freezeTimer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
            return;
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= playerDetectionRadius)
        {
            // check for line of sight to player
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized,
                out hitInfo, playerDetectionRadius, ~weepingAngelLayer, QueryTriggerInteraction.Ignore))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    // detect player
                    agent.isStopped = false;
                    agent.destination = player.transform.position;
                    agent.speed = chaseSpeed;
                }
            }  
        }
        else
        {
            agent.isStopped = true;
            agent.speed = 0f;
        }
    }

    public void Freeze()
    {
        freezeTimer = freezeDuration;
        agent.isStopped = true;
        agent.speed = 0f;
    }
}
