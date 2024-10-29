using Lurkers.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Lurkers.Environment.Hearing
{
    public class Throwable : MonoBehaviour
    {
        float throwForce = 1000f;
        Vector3 objectPos;
        float distance;

        public bool canHold = false;
        public GameObject item;
        public GameObject tempParent;
        public bool isInHand = false;
        private float holdDownStartTime;
        private Animator PlayerAnimatorController;

        void Start()
        {
            PlayerAnimatorController = tempParent.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            distance = Vector3.Distance(item.transform.position, tempParent.transform.position);
            if (distance <= 3f)
            {
                canHold = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    isInHand = !isInHand;
                    if (isInHand)
                    {
                        item.transform.position += Vector3.up;
                    }
                }
            }

            if (isInHand)
            {
                item.GetComponent<Rigidbody>().useGravity = false;
                item.GetComponent<Rigidbody>().detectCollisions = false;
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                item.transform.position = transform.position;
                //item.transform.SetParent(tempParent.transform);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    holdDownStartTime = Time.time;
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    float holdDownTime = Time.time - holdDownStartTime;
                    // item.GetComponent<Rigidbody>().AddForce(tempParent.transform.up.normalized * throwForce);
                    PlayerAnimatorController.SetTrigger("Rock Throwing");
                    item.GetComponent<Rigidbody>().AddForce((Vector3.up + GetComponent<PlayerController>().GetDir()).normalized * CalculateHoldDownForce(holdDownTime));
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

        private float CalculateHoldDownForce(float HoldTime)
        {
            float maxForceDownTime = 2f;
            float holdTimeNormalized = Mathf.Clamp01(HoldTime / maxForceDownTime);
            float force = holdTimeNormalized * throwForce;
            return force;
        }

        // void pickUpObject() 
        // {
        //     isInHand = true;
        //     item.transform.position += Vector3.up;
        //     item.GetComponent<Rigidbody>().useGravity = false;
        //     item.GetComponent<Rigidbody>().detectCollisions = true;


        // }

        // void OnMouseUp() 
        // {
        //     isInHand = false; 
        // }

    }
}
