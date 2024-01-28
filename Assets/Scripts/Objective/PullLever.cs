using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PullLever : MonoBehaviour
{
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] private AudioSource leverSource = null;
    [SerializeField] private AudioSource electricitySource = null;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (pulled)
        {
            return;
        }

        item = collision.gameObject;

        if (collision != null)
        {
            inventorySystem.OpenGUI("Press E to pull lever");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision != null)
        {
            inventorySystem.CloseGUI();
            item = null;
        }
    }

    private void LeverPulled()
    {
        onLeverPulled?.Invoke();

        // start cinematic sequence
        if (animator != null && leverSource != null && electricitySource != null)
        {
            pulled = true;
            inventorySystem.CloseGUI();
            item = null;

            animator.SetTrigger("Pulled");
            leverSource.Play();
            electricitySource.Play();
        }
        else
        {
            Debug.LogError("One of the fields is null!");
        }
    }
}
