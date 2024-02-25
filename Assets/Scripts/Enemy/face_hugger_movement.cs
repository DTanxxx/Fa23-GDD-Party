using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class face_hugger_movement : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float freezeDuration = 2f;
    [SerializeField] private LayerMask faceHuggerLayer;
    [SerializeField] private Animator animator = null;

    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Collider myCollider = null;

    private GameObject player;
    private bool isPlayerDead = false;
    private NavMeshAgent agent;
    private float freezeTimer;
    private bool enemyActive = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        SetActive(); 
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
        if (isPlayerDead || !enemyActive)
        {
            return;
        }

        if (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
            return;
        }
        if (enemyActive) 
        {
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

    private void OnPlayerDeath(Vector3 enemyPosition)
    {
        spriteRenderer.enabled = false;
        myCollider.enabled = false;
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        isPlayerDead = true;
    }

    public void SetActive()
    {
        Debug.Log("SET ACTIVE");
        enemyActive = true;
        // animation transition
        animator.SetTrigger("Activate");
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
