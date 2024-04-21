using Lurkers.Control;
using Lurkers.Hearing;
using Lurkers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

public class MothManController : MonoBehaviour, Listen
{
    [SerializeField] private float playerDetectionRadius = 7f;
    [SerializeField] private float chaseSpeed = 6f;
    [SerializeField] private float freezeDuration = 5f;
    [SerializeField] private LayerMask mothManLayer;
    [SerializeField] private Animator animator = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Collider myCollider = null;
    [SerializeField] private Transform startPatrol = null;
    [SerializeField] private Transform endPatrol = null;
    [SerializeField] private List<Sound.SoundType> InterestingSounds;


    private GameObject player;
    private bool isPlayerDead = false;
    private NavMeshAgent agent;
    private float freezeTimer;
    private bool idle = true;
    private bool enemyActive = false;
    private bool inMonologue;
    private bool enraged = false;
    private bool sToe = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    private void OnEnable()
    {
        PlayerHealth.onDeath += OnPlayerDeath;
        Dialogue.active += DialogueActive;
        Dialogue.unactive += DialogueInactive;
    }

    private void OnDisable()
    {
        PlayerHealth.onDeath -= OnPlayerDeath;
        Dialogue.active -= DialogueActive;
        Dialogue.unactive -= DialogueInactive;
    }

    private void Update()
    {
        if (isPlayerDead || !enemyActive || inMonologue)
        {
            return;
        }

        if (enraged)
        {
            if (freezeTimer > 0)
            {
                freezeTimer -= Time.deltaTime;
                return;
            }
        }


        if ((agent.destination.x - transform.position.x) > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnPlayerDeath(DeathCause cause, Vector3 enemyPosition, GameObject enemy = null)
    {
        if (cause == DeathCause.MOTHMAN && enemy == gameObject)
        {
            spriteRenderer.enabled = false;
        }

        myCollider.enabled = false;
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        isPlayerDead = true;
    }

    public void RespondToSound(Sound sound)
    {
        if (InterestingSounds.Contains(sound.soundType))
        {
            MoveTo(sound.pos);
        }
        
        Debug.Log(name + " responding to sound at " + sound.pos);
    }

    private void MoveTo(Vector3 pos)
    {
        if (!enraged)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= playerDetectionRadius)
            {
                animator.SetBool("Freeze", false);
                agent.SetDestination(pos);
                agent.isStopped = false;
                agent.speed = chaseSpeed;
            }

            else
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    animator.SetBool("Freeze", false);
                    if (sToe)
                    {
                        agent.SetDestination(endPatrol.position);
                    }
                    else
                    {
                        agent.SetDestination(startPatrol.position);
                    }
                    agent.isStopped = false;
                    agent.speed = chaseSpeed;
                }
            }
        }

        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Vector3 randDirection = Random.insideUnitSphere * 10f;
                randDirection += transform.position;
                NavMeshHit navHit;
                NavMesh.SamplePosition(randDirection, out navHit, 10f, -1);
                agent.SetDestination(navHit.position);
            }

            agent.speed = chaseSpeed;
        }
    }

    public void SetActive()
    {
        enemyActive = true;
        // animation transition
        animator.SetTrigger("Activate");
    }

    public void SetInActive()
    {
        enemyActive = false;
    }

    public void OnHit()
    {
        freezeTimer = freezeDuration;
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        // animation transition (with antennae -> no antennae and mad)
        animator.SetBool("Hit", true);
        enraged = true;
    }

    public bool IsActive()
    {
        return enemyActive;
    }

    public void DialogueActive()
    {
        Debug.Log("WeepingAngelDialogue");
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        inMonologue = true;
    }

    public void DialogueInactive()
    {
        agent.isStopped = false;
        inMonologue = false;
    }
}
