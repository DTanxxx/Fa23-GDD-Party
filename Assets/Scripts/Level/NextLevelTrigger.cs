using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Lurkers.Audio;
using Lurkers.Environment.Vision;

namespace Lurkers.Control.Level
{
    public class NextLevelTrigger : MonoBehaviour
    {
        [SerializeField] private Image fadeOutPanel = null;
        [SerializeField] private float fadeDuration = 3f;
        [SerializeField] private float pauseBeforeRising = 1f;
        [SerializeField] private float risingDuration = 4f;
        [SerializeField] private bool isElevator = true;

        private WaitForSeconds waitForPauseBeforeRising;
        private WaitForSeconds waitForRisingDuration;
        private LevelManager levelManager;

        public static Action onBeginElevatorRise;
        public static Action onBeginLevelTransition;

        private void Start()
        {
            waitForPauseBeforeRising = new WaitForSeconds(pauseBeforeRising);
            waitForRisingDuration = new WaitForSeconds(risingDuration);
            levelManager = FindObjectOfType<LevelManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(LevelTransitionSequence());
            }
        }

        private IEnumerator LevelTransitionSequence()
        {
            onBeginLevelTransition?.Invoke();

            float totalDur = fadeDuration;
            while (totalDur > 0f)
            {
                float curAlpha = fadeOutPanel.color.a;
                curAlpha += Time.deltaTime / totalDur;
                fadeOutPanel.color = new Color(fadeOutPanel.color.r,
                    fadeOutPanel.color.g, fadeOutPanel.color.b, curAlpha);
                totalDur -= Time.deltaTime;
                yield return null;
            }
            fadeOutPanel.color = new Color(fadeOutPanel.color.r,
                    fadeOutPanel.color.g, fadeOutPanel.color.b, 1f);

            yield return waitForPauseBeforeRising;

            if (isElevator)
            {
                // play rising sfx
                AudioManager.instance.SetPlayOneShot(FMODEvents.instance.elevator, transform, "Elevator", (float)Elevator.State.MOVING);
                yield return waitForRisingDuration;
            }
            
            levelManager.LoadNextScene();
        }
    }
}
