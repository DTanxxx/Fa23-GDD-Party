using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Lock[] locks; 
    private bool isOpen = false;
    private bool isTouching = false;
    //private const int numLocks = 4;


    // Start is called before the first frame update
    void Start()
    {
        //temp population of locks
        for (int i = 0; i < locks.Length; i++)
        {
            locks[i] = new Lock();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouching)
        {
            openLock(); 
        }

        isOpen = locks.All(x => !x.getLocked);
        if (isOpen)
        {
            Debug.Log("door open" + isOpen);
            GetComponent<SpriteRenderer>().enabled = !isOpen;
        } 
    }

    void openLock()
    {
        //check which lock to dissolve
        if (Input.anyKeyDown)
        {
            string someKey = Input.inputString;
            if (!string.IsNullOrEmpty(someKey)) 
            {

                Flavor someFlavor = new Flavor();
                //fetch flavor stor in someFlavor
                Debug.Log("Key Pressed " + someKey);

                for (int i = 0; i < locks.Length; i++)
                {
                    int result = 0;
                    Int32.TryParse(someKey, out result);

                    //change based on input
                    if((result - 1) == i)
                    {
                        locks[i].Dissolve(someFlavor);
                        break;
                    }
                }
            }
        }
    }

    //collision check
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            isTouching = true;
            Debug.Log("in range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            isTouching = false;
            Debug.Log("out of range.");
        }
    }
}
