using Lurkers.Environment.Hearing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Lurkers.Control.Hearing
{
    public class Echolocator : MonoBehaviour
    {
        [SerializeField] private string detectableTag = "detectable";
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameObject echoContainer;
        [SerializeField] private GameObject echoText;
        [SerializeField] private float damping = 20.0f;
        [SerializeField] private float range = 20.0f;
        [SerializeField] private float echoCastRadius = 5f;

        private Vector3 currDirection;
        private Vector3 tempDirection;

        private RaycastHit[] hits;
        private RaycastHit firstHit;
        private float firstHitDistance;

        private bool isInsideTrigger;
        private Collider trigger;

        void FixedUpdate()
        {
            currDirection = playerController.getDir();
            Quaternion smoothing = Quaternion.LookRotation(currDirection);
            echoContainer.transform.rotation = Quaternion.Lerp(echoContainer.transform.rotation,
                smoothing, Time.fixedDeltaTime * damping);

            tempDirection = echoContainer.transform.rotation * Vector3.forward;

            hits = Physics.SphereCastAll(transform.position, echoCastRadius, tempDirection, range);

            bool foundHit = false;
            foreach (var hit in hits)
            {
                // check if hit's gameobject has "detectable" tag
                if (hit.transform.gameObject.CompareTag(detectableTag))
                {
                    // it is detectable
                    firstHit = hit;
                    foundHit = true;
                    firstHitDistance = firstHit.distance;
                    if (firstHitDistance < 0)
                    {
                        firstHitDistance = 0;
                    }

                    echoText.GetComponent<TextMeshProUGUI>().text =
                        firstHit.collider.gameObject.GetComponent<EchoMaterial>().materialText + "\n"
                        + firstHitDistance.ToString("F2") + "m";

                    Debug.DrawLine(transform.position, firstHit.point, Color.yellow);

                    break;
                }
            }

            if (!foundHit)
            {
                if (isInsideTrigger)
                {
                    // this happens when player is very close to a detectable object
                    echoText.GetComponent<TextMeshProUGUI>().text =
                        trigger.gameObject.GetComponent<EchoMaterial>().materialText + "\n" + "0.00m";
                }
                else
                {
                    echoText.GetComponent<TextMeshProUGUI>().text = "Out of Range";
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("detectable"))
            {
                trigger = other;
                isInsideTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("detectable"))
            {
                trigger = null;
                isInsideTrigger = false;
            }
        }
    }
}
