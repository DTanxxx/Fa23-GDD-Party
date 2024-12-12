using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using Lurkers.Event;
using Lurkers.Environment.Vision;
using Lurkers.Cam;
using Lurkers.Control.Level;
using Lurkers.UI;
using Lurkers.Taste;

namespace Lurkers.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private int accFrames = 9;
        [SerializeField] private int lookFrames = 3;
        [SerializeField] private int frameDelay = -3;
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
        [SerializeField] private GameObject[] flashlightObj;
        [SerializeField] private GameObject echolocatorContainer = null;

        public List<Flavor> flavorCombinations = new List<Flavor>();

        private int currFrames;
        private Vector3 dir;
        private int curFrameDelay;
        private Vector3 lastDirection;
        private bool isDead = false;
        private bool isFrozen = false;
        private float animSpeed;
        private float maxSpeed;
        private PlayerSprinter _playerSprinter;
        public bool inDialogue;
        public int tileTriggerCounter = 0;

        public static Action onPlayerSlide;
        public static Action onPlayerEndSlide;
        public static Action moving;
        public static Action notMoving;

        // For all other scripts to reference the Player transform in order to play non-spatial SFX
        public static Transform playerTransform;

        private void Start()
        {
            _playerSprinter = GetComponent<PlayerSprinter>();
            dir = new Vector3(1, 0, 0);
            
            if (!debugMode)
            {
                FreezePlayer();
            }

            playerTransform = transform;

            foreach (Flavor flavor in flavorCombinations)
            {
                // shows the total flavor combinations
                Debug.Log("Combination Flavor - Sweet: " + flavor.sweet);
                Debug.Log("Combination Flavor - Bitter: " + flavor.sour);
                Debug.Log("Combination Flavor - Bitter: " + flavor.bitter);
                Debug.Log("Combination Flavor - Bitter: " + flavor.salty);
                Debug.Log("Combination Flavor - Bitter: " + flavor.umami);
            }
        }

        private void OnEnable()
        {
            PlayerHealth.onDeath += TriggerDeathAnimation;
            LeverPullAnimationEvents.onBeginLeverCinematicSequence += FreezePlayer;
            ElevatorOpen.onPlayerEntrance += TransitionIntoLevel;
            CameraFollow.onCameraRestoreComplete += UnfreezePlayer;
            NextLevelTrigger.onBeginLevelTransition += FreezePlayer;
            ColorTile.onIncinerate += Incinerate;
            Dialogue.unactive += EquipEcholocator;
        }

        private void OnDisable()
        {
            PlayerHealth.onDeath -= TriggerDeathAnimation;
            LeverPullAnimationEvents.onBeginLeverCinematicSequence -= FreezePlayer;
            ElevatorOpen.onPlayerEntrance -= TransitionIntoLevel;
            CameraFollow.onCameraRestoreComplete -= UnfreezePlayer;
            NextLevelTrigger.onBeginLevelTransition -= FreezePlayer;
            ColorTile.onIncinerate -= Incinerate;
            Dialogue.unactive -= EquipEcholocator;
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
                myRigidbody.velocity = Vector3.zero;
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

            if (curFrameDelay - 5 == frameDelay)
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
                moving?.Invoke();
                dir = lastDirection.normalized;
                myRigidbody.velocity = dir * maxSpeed * (currFrames - lookFrames) / accFrames;

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
                myRigidbody.velocity = Vector3.zero;
                animator.SetBool("Walking", false);
                notMoving?.Invoke();
            }
            // Sprinting Input 
            if (Input.GetKey(KeyCode.LeftShift)) {
                _playerSprinter.Sprinting();

            }
        }

        // Method to set a new flavors ((TO DO LATER --- ADD TO PLAYER INVENTORY))
        public void SetFlavor(Flavor[] newFlavors)
        {
            // Clear current flavor combinations
            flavorCombinations.Clear();

            // Add the new flavors
            foreach (Flavor flavor in newFlavors)
            {
                flavorCombinations.Add(flavor);
            }

            Debug.Log("Total combinations:" + flavorCombinations.Count);

        }


        public void SetRunSpeed(float speed)
        {
            maxSpeed = speed;
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
            UnfreezePlayer();
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

        public void FreezePlayer()
        {
            Debug.Log("Player frozen");
            myCollider.enabled = false;
            myRigidbody.velocity = Vector3.zero;
            animSpeed = animator.speed;
            if (!isDead)
            {
                animator.speed = 0f;
            }
            isFrozen = true;
        }

        public void UnfreezePlayer()
        {
            Debug.Log("Player unfrozen");
            myCollider.enabled = true;
            myRigidbody.velocity = Vector3.zero;
            animator.speed = animSpeed;
            isFrozen = false;
        }

        public void CheckPointRevive()
        {
            isDead = false;
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
                case DeathCause.MOTHMAN:
                    animator.SetTrigger("MothmanDeath");
                    break;
                default:
                    Debug.LogError("UNKNOWN DEATH CAUSE");
                    break;
            }

            isDead = true;
            FreezePlayer();

            switch (cause)
            {
                case DeathCause.WEEPINGANGEL:
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
                    break;
                case DeathCause.FACEHUGGER:
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
                    break;
                case DeathCause.MOTHMAN:
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
                    break;
                default:
                    break;
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

        private void EquipEcholocator(DialogueType type)
        {
            if (type == DialogueType.FLASHLIGHT_BREAK)
            {
                // change animator layer
                animator.SetLayerWeight(1, 1);
                // disable flashlight
                foreach (var obj in flashlightObj)
                {
                    obj.SetActive(false);
                }
                // enable echolocator
                echolocatorContainer.SetActive(true);
            }
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
