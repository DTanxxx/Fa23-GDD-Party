using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class WeepingAngelMovement : MonoBehaviour
{
    [SerializeField] private float playerDetectionRadius = 5f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float freezeDuration = 2f;
    [SerializeField] private LayerMask weepingAngelLayer;
    [SerializeField] private Animator animator = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Collider myCollider = null;

    private GameObject player;
    private bool isPlayerDead = false;
    private NavMeshAgent agent;
    private float freezeTimer;
    private bool idle = true;
    private bool enemyActive = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        PlayerHealth.onDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        PlayerHealth.onDeath -= OnPlayerDeath;
    }

    private void Update()
    {
        if (isPlayerDead | !enemyActive)
        {
            return;
        }

        if (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
            return;
        }
        else if (!idle)
        {
            animator.SetBool("Freeze", false);
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
                    idle = false;
                    animator.SetBool("Freeze", false);

                    agent.isStopped = false;
                    agent.destination = player.transform.position;
                    agent.speed = chaseSpeed;
                    if ((agent.destination.x - transform.position.x) > 0) {
                        spriteRenderer.flipX = false;
                    }
                    else
                    {
                        spriteRenderer.flipX = true;
                    }
                }
            }  
        }
        else
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
    }

    private void OnPlayerDeath(Vector3 enemyPosition)
    {
        spriteRenderer.enabled = false;
        myCollider.enabled = false;
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        isPlayerDead = true;
    }

    public void Freeze()
    {
        freezeTimer = freezeDuration;
        agent.velocity = Vector3.zero;
        agent.isStopped = true;

        animator.SetBool("Freeze", true);
    }

    public void SetActive()
    {
        enemyActive = true;
    }

    public void SetInActive()
    {
        enemyActive = false;
    }

    public bool EnemyActivated()
    {
        return enemyActive;
    }
}
