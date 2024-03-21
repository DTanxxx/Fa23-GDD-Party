using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using FMOD.Studio;  // TODO make a script/event so that we don't have to use this namespace
using Lurkers.Vision;
using Lurkers.Audio;

namespace Lurkers.Control.Vision.Character
{
    public class FaceHuggerController : MonoBehaviour, IFlashable
    {
        [SerializeField] private float playerDetectionRadius = 5f;
        [SerializeField] private float chaseSpeed = 5f;
        [SerializeField] private LayerMask faceHuggerLayer;
        [SerializeField] private Animator animator = null;
        [SerializeField] private float deathProximity = 8f;

        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private Collider myCollider = null;

        private GameObject player;
        private bool isPlayerDead = false;
        private NavMeshAgent agent;
        private bool isDead = false;

        private EventInstance facehuggerEventInstance;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
            facehuggerEventInstance = AudioManager.instance.SetPlay(FMODEvents.instance.enemy, transform, "Enemy", (float)Audio.Enemy.EnemyAudioSources.Enemy.FACEHUGGER_RUN);
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
            if (isPlayerDead || isDead)
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

                        AudioManager.instance.SetParameter(facehuggerEventInstance, "Facehugger Chase", 1f);
                    }
                }
            }
            else
            {
                agent.velocity = Vector3.zero;
                agent.isStopped = true;

                AudioManager.instance.SetParameter(facehuggerEventInstance, "Facehugger Chase", 0f);
            }
        }

        private void OnPlayerDeath(DeathCause cause, Vector3 enemyPosition, GameObject enemy = null)
        {
            if (cause == DeathCause.FACEHUGGER && enemy == gameObject)
            {
                spriteRenderer.enabled = false;    
            }

            myCollider.enabled = false;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            isPlayerDead = true;

            gameObject.SetActive(false);
        }

        public void OnFlash()
        {
            if (Vector3.Distance(player.transform.position, transform.position) > deathProximity)
            {
                return;
            }

            myCollider.enabled = false;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            isDead = true;

            AudioManager.instance.Stop(facehuggerEventInstance);
            AudioManager.instance.SetPlayOneShot(FMODEvents.instance.enemy, transform, "Enemy", (float)Audio.Enemy.EnemyAudioSources.Enemy.FACEHUGGER_KILL);
            animator.SetTrigger("Die");
        }

        public bool IsActive()
        {
            return true;
        }
    }
}
