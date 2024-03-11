using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Lurkers.UI
{
    public class FadeInPanel : MonoBehaviour
    {
        [SerializeField] private Image fadeInPanel = null;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private float pauseBeforeElevatorOpen = 1f;
        [SerializeField] private GameObject[] buttons;

        private WaitForSeconds waitForPauseBeforeElevatorOpen;

        public static Action onBeginElevatorOpen;

        private void Start()
        {
            waitForPauseBeforeElevatorOpen = new WaitForSeconds(pauseBeforeElevatorOpen);

            StartCoroutine(OnLevelBegin());
        }

        private IEnumerator OnLevelBegin()
        {
            float totalDur = fadeDuration;
            while (totalDur > 0f)
            {
                float curAlpha = fadeInPanel.color.a;
                curAlpha -= Time.deltaTime / totalDur;
                fadeInPanel.color = new Color(fadeInPanel.color.r,
                    fadeInPanel.color.g, fadeInPanel.color.b, curAlpha);
                totalDur -= Time.deltaTime;
                yield return null;
            }
            fadeInPanel.color = new Color(fadeInPanel.color.r,
                    fadeInPanel.color.g, fadeInPanel.color.b, 0f);

            yield return waitForPauseBeforeElevatorOpen;

            // play rising animation
            onBeginElevatorOpen?.Invoke();
        }
    }
}
