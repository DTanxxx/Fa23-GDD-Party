using Lurkers.Control;
using Lurkers.Hearing;
using Lurkers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Lurkers.Control.Hearing
{
	public class CthulhuController : MonoBehaviour, Listen
	{
        // Common
		[SerializeField] private float chaseSpeed = 6f;
		[SerializeField] private float soundThreshold = 5f;
        [SerializeField] private float soundExpireDuration = 3.0f;
        [SerializeField] private float playerDetectionRadius = 7f;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider myCollider;
        [SerializeField] private GameObject startPatrol;
        [SerializeField] private GameObject endPatrol;

        // These lists of soundtypes should have no overlap
        [SerializeField] private List<Sound.SoundType> InterestingSounds;
		[SerializeField] private List<Sound.SoundType> DangerousSounds;

        // Cthulhu
        [SerializeField] private float freezeDuration = 5f;

        private float curAmplitude;
		private float soundExpiringTimer;
        private NavMeshAgent agent;
        private GameObject player;
        private bool isPlayerDead = false;
        private float freezeTimer;
        private bool idle = true;
        private bool enemyActive = false;
        private bool inMonologue;
        private bool sToe = true;

        private void Start()
		{
            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            enemyActive = true;
            MoveTo(startPatrol.transform.position);
            player = GameObject.FindGameObjectWithTag("Player");
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

            if (soundExpiringTimer > 0f)
            {
                soundExpiringTimer -= Time.deltaTime;
            }
            else
            {
                // expire curSound
                curAmplitude = 0f;
            }

            if ((agent.destination.x - transform.position.x) > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f && !(Vector3.Distance(player.transform.position, transform.position) <= playerDetectionRadius))
            {
                /*animator.SetBool("Freeze", false);*/
                if (sToe)
                {
                    MoveTo(endPatrol.transform.position);
                    sToe = false;
                }
                else
                {
                    MoveTo(startPatrol.transform.position);
                    sToe = true;
                }

                animator.SetTrigger("Walk");
            }

            if (Vector3.Distance(player.transform.position, transform.position) <= playerDetectionRadius)
            {
                animator.SetTrigger("Seek");
                MoveTo(player.transform.position);
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
            // check if sound's amplitude is detectable
            float distToSound = Vector3.Distance(transform.position, sound.pos);
            float amplitude = sound.amplitude * (1 - distToSound / sound.range);

            if (amplitude >= soundThreshold)
            {
                if (amplitude > curAmplitude)
                {
                    // detected!
                    if (InterestingSounds.Contains(sound.soundType))
                    {
                        MoveTo(sound.pos);
                    }
                    else if (DangerousSounds.Contains(sound.soundType))
                    {
                        Vector3 dir = sound.pos - transform.position;

                        MoveTo(sound.pos - (dir * 10f));
                    }

                    animator.SetTrigger("Seek");

                    Debug.Log(name + " responding to sound at " + sound.pos);
                    curAmplitude = amplitude;
                    soundExpiringTimer = soundExpireDuration;
                }
            }
            Debug.Log(name + " responding to sound at " + sound.pos);
        }

		private void MoveTo(Vector3 pos)
		{
            agent.SetDestination(pos);
            agent.isStopped = false;
            agent.speed = chaseSpeed;
        }

        public void OnHit()
        {
            freezeTimer = freezeDuration;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }

        public void DialogueActive(DialogueType type)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            inMonologue = true;
        }

        public void DialogueInactive(DialogueType type)
        {
            agent.isStopped = false;
            inMonologue = false;
        }
    }
}
