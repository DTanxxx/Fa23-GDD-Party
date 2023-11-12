using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorOpen : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private GameObject nextLevelTrigger = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip openCloseSFX = null;

    public bool goalAchieved = false;

    private void Start()
    {
        nextLevelTrigger.SetActive(false);
    }

    private void OnEnable()
    {
        PullLever.onLeverPulled += AchieveGoal;
    }

    private void OnDisable()
    {
        PullLever.onLeverPulled -= AchieveGoal;
    }

    public void AchieveGoal()
    {
        goalAchieved = true;
    }

    public void EnableNextLevelTrigger()
    {
        nextLevelTrigger.SetActive(true);
    }

    public void DisableNextLevelTrigger()
    {
        nextLevelTrigger.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!goalAchieved || !other.gameObject.CompareTag("Player"))
        {
            return;
        }

        // play animation
        audioSource.PlayOneShot(openCloseSFX);
        animator.SetTrigger("Open");
    }
}
