using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using Lurkers.Event;
using Lurkers.Environment.Vision;
using Lurkers.Camera;
using Lurkers.Environment.Vision.ColorTile;
using Lurkers.Control.Level;

namespace Lurkers.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 0.2f;
        [SerializeField] private int accFrames = 9;
        [SerializeField] private int lookFrames = 2;
        [SerializeField] private int frameDelay = 1;
        [SerializeField] private Animator animator = null;
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private Collider myCollider = null;
        [SerializeField] private Rigidbody myRigidbody = null;
        [SerializeField] private float pauseBeforeAppearance = 1f;
        [SerializeField] private Transform beginTransform = null;
        [SerializeField] private float tolerableOffset = 1f;
        [SerializeField] private float playerTransitionRate = 0.25f;
        [SerializeField] private Light2D[] lights;
        [SerializeField] private bool debugMode = false;

        private int currFrames;
        private Vector3 dir;
        private int curFrameDelay;
        private Vector3 lastDirection;
        private bool isDead = false;
        private bool isFrozen = false;
        private float animSpeed;
        private WaitForSeconds waitForPauseBeforeAppearance;
        private float tempSpeed;
        public bool inDialogue;

        public static Action onPlayerSlide;
        public static Action onPlayerEndSlide;

        // For all other scripts to reference the Player transform in order to play non-spatial SFX
        public static Transform playerTransform;

        private void Start()
        {
            dir = new Vector3(1, 0, 0);
            waitForPauseBeforeAppearance = new WaitForSeconds(pauseBeforeAppearance);
            
            if (!debugMode)
            {
                FreezePlayer();
            }

            playerTransform = transform;
        }

        private void OnEnable()
        {
            PlayerHealth.onDeath += TriggerDeathAnimation;
            LeverPullAnimationEvents.onBeginLeverCinematicSequence += FreezePlayer;
            ElevatorOpen.onPlayerEntrance += TransitionIntoLevel;
            CameraFollow.onCameraRestoreComplete += UnfreezePlayer;
            ElevatorOpen.onElevatorClose += UnfreezePlayer;
            NextLevelTrigger.onBeginLevelTransition += FreezePlayer;
            ColorTile.onIncinerate += Incinerate;
        }

        private void OnDisable()
        {
            PlayerHealth.onDeath -= TriggerDeathAnimation;
            LeverPullAnimationEvents.onBeginLeverCinematicSequence -= FreezePlayer;
            ElevatorOpen.onPlayerEntrance -= TransitionIntoLevel;
            CameraFollow.onCameraRestoreComplete -= UnfreezePlayer;
            ElevatorOpen.onElevatorClose -= UnfreezePlayer;
            NextLevelTrigger.onBeginLevelTransition -= FreezePlayer;
            ColorTile.onIncinerate -= Incinerate;
        }

        private void FixedUpdate()
        {
            if (isDead || isFrozen)
            {
                return;
            }

            if (inDialogue)
            {
                animator.SetBool("Walking", false);
                return;
            }

            currFrames++;
            if (currFrames >= accFrames)
            {
                currFrames = accFrames + 1;
            }
            if (currFrames - lookFrames < 0)
            {
                currFrames = lookFrames;
            }

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                currFrames = 0;
            }

            if (curFrameDelay == frameDelay)
            {
                lastDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                curFrameDelay = 0;
            }
            else
            {
                // skip this frame's input
                curFrameDelay++;
            }

            if (lastDirection != Vector3.zero)
            {
                // walking
                animator.SetBool("Walking", true);

                dir = lastDirection.normalized;
                gameObject.transform.Translate(dir * maxSpeed * (currFrames - lookFrames) / accFrames);

                if (dir.x > 0)
                {
                    // going right
                    spriteRenderer.flipX = true;
                }
                else if (dir.x < 0)
                {
                    // going left
                    spriteRenderer.flipX = false;
                }
            }
            else
            {
                // not walking
                animator.SetBool("Walking", false);
            }
        }

        private void TransitionIntoLevel()
        {
            //StartCoroutine(BeginTransitioning());

            // for now, simply enable flashlight
            foreach (var light in lights)
            {
                light.enabled = true;
            }

            transform.position = beginTransform.position;
        }

        /*private IEnumerator BeginTransitioning()
        {
            yield return waitForPauseBeforeAppearance;

            float dist = Vector3.Distance(transform.position, beginTransform.position);
            while (dist > tolerableOffset)
            {
                transform.position = Vector3.Lerp(transform.position, beginTransform.position, playerTransitionRate);
                dist = Vector3.Distance(transform.position, beginTransform.position);
                yield return null;
            }
        }*/

        private void FreezePlayer()
        {
            myCollider.enabled = false;
            myRigidbody.velocity = Vector3.zero;
            animSpeed = animator.speed;
            if (!isDead)
            {
                animator.speed = 0f;
            }
            isFrozen = true;
        }

        private void UnfreezePlayer()
        {
            Debug.Log("Player unfrozen");
            myCollider.enabled = true;
            myRigidbody.velocity = Vector3.zero;
            animator.speed = animSpeed;
            isFrozen = false;
        }

        private void TriggerDeathAnimation(DeathCause cause, Vector3 enemyPosition, GameObject enemy = null)
        {
            switch (cause)
            {
                case DeathCause.WEEPINGANGEL:
                    animator.SetTrigger("WeepingAngelDeath");
                    break;
                case DeathCause.FACEHUGGER:
                    animator.SetTrigger("FaceHuggerDeath");
                    break;
                case DeathCause.REDTILE:
                    animator.SetTrigger("RedDeath");
                    break;
                default:
                    Debug.LogError("UNKNOWN DEATH CAUSE");
                    break;
            }

            isDead = true;
            FreezePlayer();

            if (cause == DeathCause.WEEPINGANGEL)
            {
                if (enemyPosition.x < transform.position.x)
                {
                    // flip sprite
                    spriteRenderer.flipX = true;
                }
                else
                {
                    // don't flip
                    spriteRenderer.flipX = false;
                }
            }
            else if (cause == DeathCause.FACEHUGGER)
            {
                if (enemyPosition.x < transform.position.x)
                {
                    // don't flip
                    spriteRenderer.flipX = false;
                }
                else
                {
                    // flip sprite
                    spriteRenderer.flipX = true;
                }
            }
        }

        private void Incinerate()
        {
            TriggerDeathAnimation(DeathCause.REDTILE, transform.position, null);
        }

        private void Slide()
        {
            myRigidbody.velocity = Vector3.zero;
            isFrozen = true;  // disable player input
                              // transition to sliding animation
            animator.SetTrigger("Slide");
            onPlayerSlide?.Invoke();
        }

        private void RecoverFromSlide()
        {
            onPlayerEndSlide?.Invoke();
            animator.SetTrigger("Recover");
            isFrozen = false;
        }

        public Vector3 GetDir()
        {
            return dir;
        }

        public void Immobile(bool onoff)
        {
            if (onoff)
            {
                Slide();
            }
            else
            {
                RecoverFromSlide();
            }
        }
    }
}
