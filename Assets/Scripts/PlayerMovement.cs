using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 0.2f;
    [SerializeField] private int accFrames = 9;
    [SerializeField] private int lookFrames = 2;
    [SerializeField] private Animator animator = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    private int currFrames;
    private Vector3 velo;
    private Vector3 dir;
    // private CharacterController controller;

    private void Start()
    {
        dir = new Vector3(1, 0, 0);
    }

    private void OnEnable()
    {
        PlayerHealth.onDeath += TriggerDeathAnimation;
    }

    private void OnDisable()
    {
        PlayerHealth.onDeath -= TriggerDeathAnimation;
    }

    private void FixedUpdate()
    {
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

        Vector3 d = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // controller.Move(d * maxSpeed * currFrames / accFrames);

        if (d != Vector3.zero)
        {
            // walking
            animator.SetBool("Walking", true);
            gameObject.transform.Translate(d * maxSpeed * (currFrames - lookFrames) / accFrames);
            dir = d.normalized;
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

    private void TriggerDeathAnimation()
    {
        Debug.Log("Player died");
        animator.SetTrigger("Die");

        // SHOULD BE IN ITS OWN SCRIPT
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public Vector3 getDir()
    {
        return dir;
    }
}
