using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Echolocator : MonoBehaviour
{
    [SerializeField] private LayerMask detectableLayer;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject echoContainer;
    [SerializeField] private GameObject echoText;
    [SerializeField] private GameObject defaultHit;
    [SerializeField] private float damping = 20.0f;
    [SerializeField] private float range = 20.0f;

    private Vector3 currDirection;
    private Vector3 tempDirection;

    private RaycastHit[] hits;
    private RaycastHit firstHit;
    private float firstHitDistance;

    private bool isInsideTrigger;
    private Collider trigger;

    void Start() {
        
    }

    void FixedUpdate() {
        currDirection = playerMovement.getDir();
        Quaternion smoothing = Quaternion.LookRotation(currDirection);
        echoContainer.transform.rotation = Quaternion.Lerp(echoContainer.transform.rotation,
            smoothing, Time.fixedDeltaTime * damping);

        tempDirection = echoContainer.transform.rotation * Vector3.forward;

        hits = Physics.RaycastAll(transform.position, tempDirection, range,
            detectableLayer, QueryTriggerInteraction.Collide);

        if (hits.Length > 0)
        {
            firstHit = hits[0];
            firstHitDistance = firstHit.distance;
            if (firstHitDistance < 0)
            {
                firstHitDistance = 0;
            }

            echoText.GetComponent<TextMeshProUGUI>().text =
                firstHit.collider.gameObject.GetComponent<EchoMaterial>().materialText + "\n"
                + firstHitDistance.ToString("F2") + "m";

            Debug.DrawLine(transform.position, firstHit.point, Color.yellow);
        }
        else if (isInsideTrigger)
        {
            echoText.GetComponent<TextMeshProUGUI>().text =
                trigger.gameObject.GetComponent<EchoMaterial>().materialText + "\n" + "0.00m";
        }
        else
        {
            echoText.GetComponent<TextMeshProUGUI>().text = "Out of Range";
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
