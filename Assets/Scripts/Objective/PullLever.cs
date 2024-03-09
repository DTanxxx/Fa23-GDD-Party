using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PullLever : MonoBehaviour
{
    [SerializeField] bool triggerLightFlicker = false;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] private Animator animator;

    public static Action onLeverPulled;

    private GameObject item;
    private bool pulled = false;

    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (item != null && !pulled)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                LeverPulled();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pulled)
        {
            return;
        }

        item = other.gameObject;

        if (other != null)
        {
            inventorySystem.OpenGUI("Press E to pull lever");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            inventorySystem.CloseGUI();
            item = null;
        }
    }

    private void LeverPulled()
    {
        onLeverPulled?.Invoke();

        if (animator != null)
        {
            pulled = true;
            inventorySystem.CloseGUI();
            item = null;

            if (triggerLightFlicker)
            {
                animator.SetTrigger("Pulled");
            }
            else
            {
                animator.SetTrigger("PulledNoFlicker");
            }

            AudioManager.instance.PlayOneShot(FMODEvents.instance.lever, transform);
        }
        else
        {
            Debug.LogError("One of the fields is null!");
        }
    }
}
