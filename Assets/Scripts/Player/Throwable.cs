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
        public GameObject Solution;
        private bool sol;
        private bool done;
        public float destroytime;
        public GameObject Splash;
        private bool thrown;
        public GameObject PreSplash;
       
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
                if (Input.GetKeyDown(KeyCode.F) && !sol)
                {
                    isInHand = !isInHand;
                    if (isInHand)
                    {
                        item.transform.position += Vector3.up;
                    }
                }
            }
         
            if (Input.GetKeyDown(KeyCode.Alpha1) && !done)
            {

                sol = !sol;
                if (sol)
                {
                    Solution.SetActive(true);
                    Solution.transform.position += Vector3.up;
                }
                else
                {
                    Solution.SetActive(false);
                }
            }
            if (thrown && !done && Solution.transform.position.y < 1)
            {
                Splash.SetActive(true);
                Splash.transform.position = Solution.transform.position;
                Debug.Log("Here");
                
            }

            if (isInHand && !sol)
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
            if (sol && !done)
            {
                Solution.GetComponent<Rigidbody>().useGravity = false;
                Solution.GetComponent<Rigidbody>().detectCollisions = false;
                Solution.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Solution.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                Solution.transform.position = transform.position;
                //item.transform.SetParent(tempParent.transform);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    holdDownStartTime = Time.time;
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    
                    PreSplash.SetActive(true);
                    PreSplash.transform.position = transform.position + GetComponent<PlayerController>().GetDir() * 13;
                    //Debug.Log(GetComponent<PlayerController>().GetDir());
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    float holdDownTime = Time.time - holdDownStartTime;
                    // item.GetComponent<Rigidbody>().AddForce(tempParent.transform.up.normalized * throwForce);
                    PreSplash.SetActive(false);
                    PlayerAnimatorController.SetTrigger("Rock Throwing");
                    Solution.GetComponent<Rigidbody>().AddForce((Vector3.up + GetComponent<PlayerController>().GetDir()).normalized * 1000);
                    Debug.Log(CalculateHoldDownForce(holdDownTime));
                    Solution.GetComponent<Rigidbody>().drag = 1;
                    sol = false;
                    thrown = true;
                    StartCoroutine(SelfDestruct());
                }
            }
            else if (!done)
            {
                objectPos = Solution.transform.position;
                Solution.transform.SetParent(null);
                Solution.GetComponent<Rigidbody>().useGravity = true;
                Solution.transform.position = objectPos;
                Solution.GetComponent<Rigidbody>().detectCollisions = true;
                
            }
            
        }
        IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(destroytime);
            Destroy(Solution);
            done = true;
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
