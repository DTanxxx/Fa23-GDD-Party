using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lurkers.Audio;

namespace Lurkers.Environment.Vision
{
    public class ElevatorOpen : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private GameObject nextLevelTrigger = null;

        public bool goalAchieved = false;

        private bool opened = false;

        public static Action onElevatorClose;
        public static Action onPlayerEntrance;

        private void Start()
        {
            nextLevelTrigger.SetActive(false);
        }

        private void OnEnable()
        {
            PullLever.onLeverPulled += AchieveGoal;
            FadeInPanel.onBeginElevatorOpen += BeginElevatorOpen;
        }

        private void OnDisable()
        {
            PullLever.onLeverPulled -= AchieveGoal;
            FadeInPanel.onBeginElevatorOpen -= BeginElevatorOpen;
        }

        public void AchieveGoal()
        {
            goalAchieved = true;
        }

        // animation event
        public void EnableNextLevelTrigger()
        {
            nextLevelTrigger.SetActive(true);
        }

        // animation event
        public void DisableNextLevelTrigger()
        {
            nextLevelTrigger.SetActive(false);
        }

        // animation event
        public void ElevatorClose()
        {
            onElevatorClose?.Invoke();
        }

        // animation event
        public void ShowPlayer()
        {
            onPlayerEntrance?.Invoke();
        }

        private void BeginElevatorOpen()
        {
            // play animation
            animator.SetTrigger("Enter");
            AudioManager.instance.SetPlayOneShot(FMODEvents.instance.elevator, transform, "Elevator", (float)Elevator.State.OPEN_CLOSE);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!goalAchieved || !other.gameObject.CompareTag("Player") || opened)
            {
                return;
            }

            // play animation
            AudioManager.instance.SetPlayOneShot(FMODEvents.instance.elevator, transform, "Elevator", (float)Elevator.State.OPEN);
            animator.SetTrigger("Exit");
            opened = true;
        }
    }
}
