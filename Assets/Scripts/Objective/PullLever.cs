using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PullLever : MonoBehaviour
{
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] EnemyActivate enemyActivate;
    [SerializeField] private AudioSource leverSource = null;
    [SerializeField] private AudioSource electricitySource = null;
    [SerializeField] private Animator animator;

    public static Action onLeverPulled;
    private GameObject item;

    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        enemyActivate = FindObjectOfType<EnemyActivate>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (item != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                LeverPulled();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        item = collision.gameObject;

        Debug.Log("collide");
        if (collision != null)
        {
            Debug.Log(item);
            inventorySystem.OpenGUI("Press E to pull lever");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision != null)
        {
            Debug.Log("out");
            inventorySystem.CloseGUI();
            item = null;
        }
    }

    private void LeverPulled()
    {
        if (animator != null && leverSource != null && electricitySource != null)
        {
            animator.SetTrigger("Pulled");
            leverSource.Play();
            electricitySource.Play();
        }

        onLeverPulled?.Invoke();

        enemyActivate.SetActive();
    }
}
