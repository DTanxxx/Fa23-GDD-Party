using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundCam : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void OnPreCull()
    {
        var m = cam.transform.worldToLocalMatrix;

        m.SetColumn(2, 1e-3f * m.GetColumn(2) - new Vector4(0, 1, 0, 0));
        cam.worldToCameraMatrix = m;
    }
}
