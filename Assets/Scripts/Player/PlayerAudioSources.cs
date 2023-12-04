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
    [SerializeField] private AudioSource incinerationSource = null;

    private void Start()
    {
        StopAllSFX();
    }

    private void OnEnable()
    {
        PlayerAnimationEvents.onFootstep += PlayFootstepSFX;
        PlayerAnimationEvents.onSkullCrush += PlaySkullCrushSFX;
        EnemyProximitySensor.onEnemyInProximity += EnterPanicPhase;
        EnemyProximitySensor.onEnemyOutOfProximity += EnterCalmPhase;
        NextLevelTrigger.onBeginLevelTransition += StopAllSFX;
        ElevatorOpen.onElevatorClose += EnterCalmPhase;
        LevelManager.onPauseGame += PauseAllSFX;
        ColorTile.onIncinerate += PlayIncinerateSFX;
    }

    private void OnDisable()
    {
        PlayerAnimationEvents.onFootstep -= PlayFootstepSFX;
        PlayerAnimationEvents.onSkullCrush -= PlaySkullCrushSFX;
        EnemyProximitySensor.onEnemyInProximity -= EnterPanicPhase;
        EnemyProximitySensor.onEnemyOutOfProximity -= EnterCalmPhase;
        NextLevelTrigger.onBeginLevelTransition -= StopAllSFX;
        ElevatorOpen.onElevatorClose -= EnterCalmPhase;
        LevelManager.onPauseGame -= PauseAllSFX;
        ColorTile.onIncinerate -= PlayIncinerateSFX;
    }

    private void PauseAllSFX(bool toPause)
    {
        if (toPause)
        {
            breathSource.Pause();
            heartbeatSource.Pause();
        }
        else
        {
            breathSource.UnPause();
            heartbeatSource.UnPause();
        }
    }

    private void PlayIncinerateSFX()
    {
        incinerationSource.Play();
    }

    private void StopAllSFX()
    {
        breathSource.Stop();
        heartbeatSource.Stop();
    }

    private void EnterPanicPhase()
    {
        if (breathSource.clip == breathClips[1])
        {
            // already in panic phase, return
            return;
        }

        breathSource.clip = breathClips[1];
        heartbeatSource.clip = heartbeatClips[2];

        breathSource.Play();
        heartbeatSource.Play();
    }

    private void EnterCalmPhase()
    {
        if (breathSource.clip == breathClips[0])
        {
            // already in calm phase, return
            return;
        }

        breathSource.clip = breathClips[0];
        heartbeatSource.clip = heartbeatClips[0];

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
