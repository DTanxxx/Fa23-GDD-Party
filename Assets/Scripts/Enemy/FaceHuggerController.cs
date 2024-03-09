using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lurkers.Vision;

namespace Lurkers.Control.Vision.Character
{
    public class FaceHuggerController : MonoBehaviour, IFlashable
    {
        [SerializeField] private float playerDetectionRadius = 5f;
        [SerializeField] private float chaseSpeed = 5f;
        [SerializeField] private LayerMask faceHuggerLayer;
        [SerializeField] private Animator animator = null;

        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private Collider myCollider = null;

        private GameObject player;
        private bool isPlayerDead = false;
        private NavMeshAgent agent;

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
            if (isPlayerDead)
            {
                return;
            }

            if (Vector3.Distance(player.transform.position, transform.position) <= playerDetectionRadius)
            {
                // check for line of sight to player
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized,
                    out hitInfo, playerDetectionRadius, ~faceHuggerLayer, QueryTriggerInteraction.Ignore))
                {
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        // detect player
                        animator.SetTrigger("Chase");

                        agent.isStopped = false;
                        agent.destination = player.transform.position;
                        agent.speed = chaseSpeed;
                        if ((agent.destination.x - transform.position.x) > 0)
                        {
                            spriteRenderer.flipX = true;
                        }
                        else
                        {
                            spriteRenderer.flipX = false;
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

        private void OnPlayerDeath(Vector3 enemyPosition, DeathCause cause)
        {
            spriteRenderer.enabled = false;
            myCollider.enabled = false;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            isPlayerDead = true;
        }

        public void OnFlash()
        {
            // kill this enemy (TODO need animation + SFX)
            Destroy(gameObject);
        }
    }
}
