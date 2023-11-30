using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 0.2f;
    [SerializeField] private int accFrames = 9;
    [SerializeField] private int lookFrames = 2;
    [SerializeField] private int frameDelay = 1;
    [SerializeField] private Animator animator = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Collider myCollider = null;
    [SerializeField] private Rigidbody myRigidbody = null;

    private int currFrames;
    private Vector3 velo;
    private Vector3 dir;
    private int curFrameDelay;
    private Vector3 lastDirection;
    private bool isDead = false;
    private bool isFrozen = false;
    private float animSpeed;

    private void Start()
    {
        dir = new Vector3(1, 0, 0);
    }

    private void OnEnable()
    {
        PlayerHealth.onDeath += TriggerDeathAnimation;
        LeverPullAnimationEvents.onBeginLeverCinematicSequence += FreezePlayer;
        CameraFollow.onCameraRestoreComplete += UnfreezePlayer;
    }

    private void OnDisable()
    {
        PlayerHealth.onDeath -= TriggerDeathAnimation;
        LeverPullAnimationEvents.onBeginLeverCinematicSequence -= FreezePlayer;
        CameraFollow.onCameraRestoreComplete -= UnfreezePlayer;
    }

    private void FixedUpdate()
    {
        if (isDead || isFrozen)
        {
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

        // controller.Move(d * maxSpeed * currFrames / accFrames);

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

    private void FreezePlayer()
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

    private void UnfreezePlayer()
    {
        Debug.Log("Player unfrozen");
        myCollider.enabled = true;
        myRigidbody.velocity = Vector3.zero;
        animator.speed = animSpeed;
        isFrozen = false;
    }

    private void TriggerDeathAnimation(Vector3 enemyPosition)
    {
        Debug.Log("Player died");
        animator.SetTrigger("Die");
        isDead = true;
        FreezePlayer();
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

    public Vector3 getDir()
    {
        return dir;
    }
}
