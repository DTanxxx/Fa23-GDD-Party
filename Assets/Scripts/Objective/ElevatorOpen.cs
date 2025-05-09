using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lurkers.Audio;
using Lurkers.UI;

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
        public static Action onFirstTimeClose;

        private static bool firstTimeClose = true;
        private bool elevatorSequenceOver = true;

        private void Start()
        {
            nextLevelTrigger.SetActive(false);
        }
        
        // added update loop to check and skip the beginning animation sequence. 
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !elevatorSequenceOver)
            {
                SkipStartAnimation();
                elevatorSequenceOver = true;
            }
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
            elevatorSequenceOver = true;
            onElevatorClose?.Invoke();

            if (firstTimeClose)
            {
                firstTimeClose = false;
                // show intro dialogue
                onFirstTimeClose?.Invoke();
            }
        }

        // animation event
        public void ShowPlayer()
        {
            onPlayerEntrance?.Invoke();
        }

        private void BeginElevatorOpen()
        {
            // play animation
            elevatorSequenceOver = false;
            animator.SetTrigger("Enter");
            AudioManager.instance.SetPlayOneShot(FMODEvents.instance.elevator, AudioManager.instance.playerTransform(), "Elevator", (float)Elevator.State.OPEN_CLOSE);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!goalAchieved || !other.gameObject.CompareTag("Player") || opened)
            {
                return;
            }

            // play animation
            AudioManager.instance.SetPlayOneShot(FMODEvents.instance.elevator, AudioManager.instance.playerTransform(), "Elevator", (float)Elevator.State.OPEN);
            animator.SetTrigger("Exit");
            opened = true;
        }

        private void SkipStartAnimation() {
            animator.Play("Entry", -1, 1f);
            ShowPlayer();
            ElevatorClose();
        }
    }
}
