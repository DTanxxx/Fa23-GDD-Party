using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioSources : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource = null;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioSource breathSource = null;
    [SerializeField] private AudioClip[] breathClips;
    [SerializeField] private AudioSource heartbeatSource = null;
    [SerializeField] private AudioClip[] heartbeatClips;
    [SerializeField] private AudioSource skullCrushSource = null;

    private void Start()
    {
        breathSource.clip = breathClips[0];
        heartbeatSource.clip = heartbeatClips[0];

        breathSource.Play();
        heartbeatSource.Play();
    }

    private void Update()
    {
        // TEMPORARY CODE TO TEST OUT DIFFERENT SFX
        if (Input.GetKeyDown(KeyCode.P))
        {
            // P for panic
            EnterPanicPhase();
        }
    }

    private void OnEnable()
    {
        PlayerAnimationEvents.onFootstep += PlayFootstepSFX;
        PlayerAnimationEvents.onSkullCrush += PlaySkullCrushSFX;
    }

    private void OnDisable()
    {
        PlayerAnimationEvents.onFootstep -= PlayFootstepSFX;
        PlayerAnimationEvents.onSkullCrush -= PlaySkullCrushSFX;
    }

    private void EnterPanicPhase()
    {
        breathSource.clip = breathClips[1];
        heartbeatSource.clip = heartbeatClips[2];

        breathSource.Play();
        heartbeatSource.Play();
    }

    private void PlayFootstepSFX()
    {
        int index = Random.Range(0, footstepClips.Length);
        footstepSource.PlayOneShot(footstepClips[index]);
    }

    private void PlaySkullCrushSFX()
    {
        footstepSource.Stop();
        breathSource.Stop();
        heartbeatSource.Stop();
        skullCrushSource.Play();
    }
}
