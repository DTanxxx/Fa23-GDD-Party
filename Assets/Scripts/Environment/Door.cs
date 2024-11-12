using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Lock[] locks;
    [SerializeField] private Flavor reqFlavor;
    private bool isOpen = false;
    private bool isTouching = false;
    //private const int numLocks = 4;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < locks.Length; i++)
        {
            locks[i] = new Lock();
        }
        
        Flavor firstFlav = new Flavor();
        firstFlav.bitter = 100;
        firstFlav.salty = 50;
        locks[0].setFlav(firstFlav);    
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
            int keyNum = Int32.Parse(someKey);
            if (!string.IsNullOrEmpty(someKey)) 
            {
                //fetch flavor store in someFlavor
                Debug.Log("Key Pressed " + someKey);


                for (int i = 0; i < locks.Length; i++)
                {
                    if (locks[i].getLocked && (keyNum <= locks.Length) && (keyNum >= 0))
                    {
                        Debug.Log("correct input");
                        locks[keyNum].Dissolve(reqFlavor);
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
