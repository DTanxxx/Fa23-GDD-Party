using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private Image fadeOutPanel = null;
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private float pauseBeforeRising = 1f;
    [SerializeField] private float risingDuration = 4f;

    private WaitForSeconds waitForPauseBeforeRising;
    private WaitForSeconds waitForRisingDuration;
    private AudioSource audioSource;
    private LevelManager levelManager;

    public static Action onBeginElevatorRise;
    public static Action onBeginLevelTransition;

    private void Start()
    {
        waitForPauseBeforeRising = new WaitForSeconds(pauseBeforeRising);
        waitForRisingDuration = new WaitForSeconds(risingDuration);
        audioSource = GetComponent<AudioSource>();
        levelManager = GetComponent<LevelManager>();
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

        // play rising sfx
        audioSource.Play();

        yield return waitForRisingDuration;

        audioSource.Stop();
        levelManager.LoadNextScene();
    }
}
