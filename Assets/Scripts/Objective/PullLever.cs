using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PullLever : MonoBehaviour
{
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] private AudioSource leverSource = null;
    [SerializeField] private AudioSource electricitySource = null;
    [SerializeField] private Animator animator;

    public static Action onLeverPulled;

    private GameObject item;
    private bool pulled = false;
    public Dialogue Monologue;

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

        // start cinematic sequence
        if (animator != null && leverSource != null && electricitySource != null)
        {
            pulled = true;
            inventorySystem.CloseGUI();
            item = null;

            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                animator.SetTrigger("Pulled2");
                FindObjectOfType<EnemyActivate>().SetActive();
            }
            else
            {
                animator.SetTrigger("Pulled");
            }
            
            leverSource.Play();
            electricitySource.Play();
            FindObjectOfType<EnemyActivate>().SetActive();
            string[] lines = new string[3];
            lines[0] = "Phew. Now that’s done.";
            lines[1] = "Did that statue.. Just move?";
            lines[2] = "Ahh, they’re alive!";
            Monologue.lines = lines;
            Monologue.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("One of the fields is null!");
        }
    }
}
