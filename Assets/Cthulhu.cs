using Lurkers.Control;
using Lurkers.Hearing;
using Lurkers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

namespace Lurkers.Control.Hearing.Character
{
    public class Cthulhu: MonoBehaviour, Listen
    {
        private NavMeshAgent agent;
        [SerializeField] private float chaseSpeed = 6f;
        [SerializeField] private float soundThreshold = 5f;
        [SerializeField] private float soundExpireDuration = 3.0f;
        // public float listeningRange = 50f;

        //added
        [SerializeField] private float playerDetectionRadius = 7f;
        [SerializeField] private float freezeDuration = 5f;
        [SerializeField] private LayerMask mothManLayer;
        [SerializeField] private GameObject animatorObj;
        [SerializeField] private GameObject spriteRendererObj;
        [SerializeField] private Collider myCollider;
        [SerializeField] private GameObject startPatrol;
        [SerializeField] private GameObject endPatrol;


        // These lists of soundtypes should have no overlap
        [SerializeField] private List<Sound.SoundType> InterestingSounds;
        [SerializeField] private List<Sound.SoundType> DangerousSounds;

        private float curAmplitude; 
        private float soundExpiringTimer;

        //added
        private GameObject player;
        private bool isPlayerDead = false;
        private float freezeTimer;
        private bool idle = true;
        private bool enemyActive = false;
        private bool inMonologue;
        private Animator animator;
        /*private bool enraged = false;*/
        private bool sToe = true;
        private SpriteRenderer spriteRenderer;
        private bool check;
        private void Start()
        {

            agent = GetComponentInParent<NavMeshAgent>();

            //added
            agent.autoBraking = false;
            player = GameObject.FindGameObjectWithTag("Player");
            myCollider = GetComponent<Collider>();
            spriteRenderer = spriteRendererObj.GetComponent<SpriteRenderer>();
            animator = animatorObj.GetComponent<Animator>();
            /*InterestingSounds = new List<Sound.SoundType>();
			InterestingSounds.Add(Sound.SoundType.Footstep);
			InterestingSounds.Add(Sound.SoundType.Rock);*/
            enemyActive = true;
            DangerousSounds = new List<Sound.SoundType>();
            DangerousSounds.Add(Sound.SoundType.Bats);



        }
        //added OnEnable and OnDisable
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
            /*            if (soundExpiringTimer > 0f)
                        {
                            soundExpiringTimer -= Time.deltaTime;
                        }
                        else
                        {
                            // expire curSound
                            curAmplitude = 0f;
                        }*/
            if (isPlayerDead || !enemyActive || inMonologue)
            {
                return;
            }

            /*if (enraged)
            {
                if (freezeTimer > 0)
                {
                    freezeTimer -= Time.deltaTime;
                    return;
                }
            }*/


            if ((agent.destination.x - transform.position.x) > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
            //added
            if (!agent.pathPending && agent.remainingDistance < 0.5f && !(Vector3.Distance(player.transform.position, transform.position) <= playerDetectionRadius))
            {
                /*animator.SetBool("Freeze", false);*/
                Debug.Log(check);
                if (sToe)
                {
                    agent.SetDestination(endPatrol.transform.position);
                    sToe = false;
                }
                else
                {
                    agent.SetDestination(startPatrol.transform.position);
                    sToe = true;
                }
                agent.isStopped = false;
                agent.speed = chaseSpeed;
                animator.SetBool("Run   ", false);
                animator.SetBool("Sound", false);
                animator.SetBool("Walk", true);

            }
            if (Vector3.Distance(player.transform.position, transform.position) <= playerDetectionRadius)
            {
                agent.SetDestination(player.transform.position);
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
            /*// check if sound's amplitude is detectable
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

					Debug.Log(name + " responding to sound at " + sound.pos);
					curAmplitude = amplitude;
					soundExpiringTimer = soundExpireDuration;
				}
			}*/
            if (InterestingSounds.Contains(sound.soundType))
            {
                // animator.SetBool("Sound", true);
                MoveTo(sound.pos);
                
            }

            Debug.Log(name + " responding to sound at " + sound.pos);
        }

        private void MoveTo(Vector3 pos)
        {
            // might want to check if the player is in LOS or if the distance the agent needs to // travel is within a certain range
            /*			agent.SetDestination(pos);
                        agent.isStopped = false;
                        agent.speed = chaseSpeed;*/
            //added
            if (Vector3.Distance(player.transform.position, transform.position) <= playerDetectionRadius)
            {
               
                agent.SetDestination(pos);
                agent.isStopped = false;
                agent.speed = chaseSpeed;
            }
        }

        //added all functions added
        public void SetActive()
        {
            enemyActive = true;
            // animation transition
            /*animator.SetTrigger("Activate");*/
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
            /*animator.SetBool("Hit", true);*/
            /*            enraged = true;*/
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
