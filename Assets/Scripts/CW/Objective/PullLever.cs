using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PullLever : MonoBehaviour
{
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] EnemyActivate enemyActivate;
    [SerializeField] private AudioSource leverSource = null;
    [SerializeField] private AudioSource electricitySource = null;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        enemyActivate = FindObjectOfType<EnemyActivate>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log('e');
            LeverPulled();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide");
        if (collision != null)
        {
            Debug.Log("touch");
            inventorySystem.OpenGUI("Press E to pull lever");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision != null)
        {
            Debug.Log("out");
            inventorySystem.CloseGUI();
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

        enemyActivate.SetActive();
    }
}
