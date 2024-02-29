using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwing_script : MonoBehaviour
{
    float throwForce = 500; 
    Vector3 objectPos;
    float distance;

    public bool canHold = true; 
    public GameObject item; 
    public GameObject tempParent; 
    public bool isHolding = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (isHolding && Input.GetKeyDown(Keycode.Space))
        {
            
        }
    }

    private void Throw() 
    {
        readyToThrow = false; 

        GameObject projectile = Instantiate()
    }
}
