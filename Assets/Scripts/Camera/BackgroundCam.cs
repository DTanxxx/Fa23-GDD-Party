using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCam : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
    }
}
