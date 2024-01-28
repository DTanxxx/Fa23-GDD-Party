using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudio : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayerAnimationEvents.onEndPlayerDeathAnim += PlayGameOverSFX;
    }

    private void OnDisable()
    {
        PlayerAnimationEvents.onEndPlayerDeathAnim -= PlayGameOverSFX;
    }

    private void PlayGameOverSFX()
    {
        audioSource.Play();
    }
}
