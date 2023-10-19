using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private float degrees;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 getPosition() 
    { 
        return position; 
    }

    public float getDegrees() 
    {
        return degrees;
    }
}
