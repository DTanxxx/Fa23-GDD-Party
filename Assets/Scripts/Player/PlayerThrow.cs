using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Control
{
    public class PlayerThrow : MonoBehaviour
    {
        [SerializeField] private float destroyTime = 3f;
        [SerializeField] private float throwForce = 1000f;

        Vector3 objectPos;

        public GameObject tempParent;
        private float holdDownStartTime;
        private Animator PlayerAnimatorController;
        
        //public GameObject Splash;
        private bool thrown;
        //public GameObject PreSplash;
        private GameObject toThrow;
       
        private void Start()
        {
            PlayerAnimatorController = tempParent.GetComponent<Animator>();
        }

        private void Update()
        {
            if (toThrow == null)
            {
                return;
            }

            if (thrown && toThrow.transform.position.y < 1)
            {
                //Splash.SetActive(true);
                //Splash.transform.position = toThrow.transform.position;             
            }

            if (!thrown)
            {
                toThrow.GetComponent<Rigidbody>().useGravity = false;
                toThrow.GetComponent<Rigidbody>().detectCollisions = false;
                toThrow.GetComponent<Rigidbody>().velocity = Vector3.zero;
                toThrow.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                toThrow.transform.position = transform.position;
                //item.transform.SetParent(tempParent.transform);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    holdDownStartTime = Time.time;
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    //PreSplash.SetActive(true);
                   //PreSplash.transform.position = transform.position + GetComponent<PlayerController>().GetDir() * 13;
                    //Debug.Log(GetComponent<PlayerController>().GetDir());
                }
                else if (Input.GetKeyUp(KeyCode.Space))
                {
                    float holdDownTime = Time.time - holdDownStartTime;
                    // item.GetComponent<Rigidbody>().AddForce(tempParent.transform.up.normalized * throwForce);
                    //PreSplash.SetActive(false);
                    PlayerAnimatorController.SetTrigger("Rock Throwing");
                    float throwForce = CalculateHoldDownForce(holdDownTime);
                    toThrow.GetComponent<Rigidbody>().AddForce((Vector3.up + GetComponent<PlayerController>().GetDir()).normalized * throwForce);
                    thrown = true;
                    SelfDestruct();
                }
            }
            else
            {
                objectPos = toThrow.transform.position;
                toThrow.GetComponent<Rigidbody>().useGravity = true;
                toThrow.transform.position = objectPos;
                toThrow.GetComponent<Rigidbody>().detectCollisions = true;
            }
        }

        private void SelfDestruct()
        {
            Destroy(toThrow, destroyTime);
        }

        private float CalculateHoldDownForce(float HoldTime)
        {
            float maxForceDownTime = 2f;
            float holdTimeNormalized = Mathf.Clamp01(HoldTime / maxForceDownTime);
            float force = holdTimeNormalized * throwForce;
            return force;
        }

        public void Equip(GameObject obj)
        {
            thrown = false;
            toThrow = obj;
        }
    }
}
