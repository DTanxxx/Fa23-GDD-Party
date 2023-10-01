using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Timeline;

public class LightDirection : MonoBehaviour
{
    private Movement parentScript;
    private Vector3 currDir;
    private float damping = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        parentScript = GetComponentInParent<Movement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currDir = parentScript.getDir();
        Quaternion smoothing = Quaternion.LookRotation(currDir);
        gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, smoothing,
            Time.fixedDeltaTime * damping);
    }
}
