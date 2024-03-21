using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lurkers.Vision;
using Lurkers.UI;

namespace Lurkers.Control.Vision.Character
{
    public class WeepingAngelController : MonoBehaviour, IFlashable
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
        private bool inMonologue;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
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
                        if ((agent.destination.x - transform.position.x) > 0)
                        {
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

        private void OnPlayerDeath(DeathCause cause, Vector3 enemyPosition, GameObject enemy = null)
        {
            if (cause == DeathCause.WEEPINGANGEL && enemy == gameObject)
            {
                spriteRenderer.enabled = false;
            }

            myCollider.enabled = false;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            isPlayerDead = true;
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

        public void OnFlash()
        {
            freezeTimer = freezeDuration;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;

            animator.SetBool("Freeze", true);
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
}
