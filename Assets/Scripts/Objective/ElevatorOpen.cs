using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorOpen : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private GameObject nextLevelTrigger = null;

    private bool goalAchieved = false;

    private void Start()
    {
        nextLevelTrigger.SetActive(false);
    }

    public void AchieveGoal()
    {
        goalAchieved = true;
    }

    public void EnableNextLevelTrigger()
    {
        nextLevelTrigger.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!goalAchieved || !other.gameObject.CompareTag("Player"))
        {
            return;
        }

        // play animation
        animator.SetTrigger("Open");
    }
}
