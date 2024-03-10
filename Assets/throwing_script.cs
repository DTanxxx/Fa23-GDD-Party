using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwing_script : MonoBehaviour
{
    float throwForce = 500f; 
    Vector3 objectPos;
    float distance;

    public bool canHold = false; 
    public GameObject item; 
    public GameObject tempParent; 
    public bool isInHand = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        distance = Vector3.Distance (item.transform.position, tempParent.transform.position); 
        if (distance <= 5f) 
        {
            canHold = true; 
            if (Input.GetKeyDown(KeyCode.F)) {
            isInHand = !isInHand;
        }
        }
        
        

        if (isInHand)
        {
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().detectCollisions = false; 
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.up * throwForce);
                item.GetComponent<Rigidbody>().drag = 1;
                isInHand = false; 
            }
        }
        else 
        {
            objectPos = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true; 
            item.transform.position = objectPos;
            item.GetComponent<Rigidbody>().detectCollisions = true;

        }

    }

    void pickUpObject() 
    {
        isInHand = true;
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().detectCollisions = true;
        
       
    }

    void OnMouseUp() 
    {
        isInHand = false; 
    }

}
