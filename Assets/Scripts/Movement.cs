using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float maxSpeed = 0.2f;
    private int accFrames = 9;
    private int lookFrames = 2;
    private int currFrames;
    private Vector3 velo;
    private Vector3 dir;
    // private CharacterController controller;

    private void Start()
    {
        dir = new Vector3(1, 0, 0);
    }

    void FixedUpdate()
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
            gameObject.transform.Translate(d * maxSpeed * (currFrames - lookFrames) / accFrames);
            dir = d.normalized;
        }
    }

    public Vector3 getDir()
    {
        return dir;
    }
}
