using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using Lurkers.Environment.Lock;

public class Door : MonoBehaviour
{
    private Lock[] locks; 
    private bool isOpen = false;
    private bool isTouching = false;
    private static int numLocks = 4;


    // Start is called before the first frame update
    void Start()
    {
        locks = new Lock[numLocks];

        //populate
        for (int i = 0; i <= numLocks; i++)
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

        isOpen = locks.All(x => x.isLocked);
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

                //unlocks lock if pressing number
                Debug.Log("Key Pressed " + someKey);

                switch (someKey)
                {
                    case "1":
                        locks[0].isLocked = true;
                        break;
                    case "2":
                        locks[1].isLocked = true;
                        break;
                    case "3":
                        locks[2].isLocked = true;
                        break;
                    case "4":
                        locks[3].isLocked = true;
                        break;
                    default:
                        Debug.Log("no key match");
                        break;
                }
                //Debug.Log("key opened: #" + someKey);
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
