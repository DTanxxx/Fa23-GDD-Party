using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    [SerializeField] private float degrees;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public Vector3 GetPosition() 
    { 
        return transform.position; 
    }

    public float GetDegrees() 
    {
        return degrees;
    }
}
