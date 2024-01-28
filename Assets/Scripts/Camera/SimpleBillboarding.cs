using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBillboarding : MonoBehaviour
{
    [SerializeField] private bool freezeXZAxis = true;
    [SerializeField] private float lightFacingFactor = 0.8f;

    private void Update()
    {
        /*if (freezeXZAxis)
        {
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Camera.main.transform.rotation;
        }*/
        Transform resultT = Camera.main.transform;
        resultT.position = (1 - lightFacingFactor) * resultT.position + lightFacingFactor * FindObjectOfType<LightDirection>().transform.position;

        transform.LookAt(resultT);
        transform.Rotate(0, 180, 0);
    }
}
