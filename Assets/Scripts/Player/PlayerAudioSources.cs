using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class PlayerAudioSources : MonoBehaviour
{
    /*[SerializeField] private AudioSource footstepSource = null;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioSource breathSource = null;
    [SerializeField] private AudioClip[] breathClips;
    [SerializeField] private AudioSource heartbeatSource = null;
    [SerializeField] private AudioClip[] heartbeatClips;
    [SerializeField] private AudioSource skullCrushSource = null;
    [SerializeField] private AudioSource incinerationSource = null;
    [SerializeField] private AudioSource slideSource = null;*/

    public static EventInstance breathingAndHeartbeat;
    public static EventInstance death;
    private enum DeathType
    {
        SIGHT_MONSTER = 0,
        RED_TILE = 1,
    }

    private void OnEnable()
    {
        PlayerAnimationEvents.onFootstep += PlayFootstepSFX;
        PlayerAnimationEvents.onSkullCrush += PlaySkullCrushSFX;
        /*PlayerMovement.onPlayerSlide += PlaySlideSFX;
        PlayerMovement.onPlayerEndSlide += StopSlideSFX;*/
        PlayerAnimationEvents.onEndPlayerDeathAnim += PlayGameOverSFX;
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
        /*PlayerMovement.onPlayerSlide -= PlaySlideSFX;
        PlayerMovement.onPlayerEndSlide -= StopSlideSFX;*/
        PlayerAnimationEvents.onEndPlayerDeathAnim -= PlayGameOverSFX;
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
            AudioManager.instance.PauseAll();
        }
        else
        {
            AudioManager.instance.UnpauseAll();
        }
    }

/*    private void PlayIncinerateSFX()
    {
        incinerationSource.Play();
    }

    private void PlaySlideSFX()
    {
        slideSource.Play();
    }

    private void StopSlideSFX()
    {
        slideSource.Stop();
    }*/

    private void StopAllSFX()
    {
        AudioManager.instance.StopAll();
    }

    private void EnterPanicPhase()
    {
        AudioManager.instance.SetParameter(breathingAndHeartbeat, "Intensity", 1f);
    }

    private void EnterCalmPhase()
    {
        AudioManager.instance.SetParameter(breathingAndHeartbeat, "Intensity", 0f);
    }

    private void PlayFootstepSFX()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.footsteps, transform);
    }

    private void PlaySkullCrushSFX()
    {
        StopAllSFX();
        AudioManager.instance.SetPlayOneShot(FMODEvents.instance.death, transform, "DeathType", (float)DeathType.SIGHT_MONSTER);
    }

    private void PlayIncinerateSFX()
    {
        StopAllSFX();
        AudioManager.instance.SetPlayOneShot(FMODEvents.instance.death, transform, "DeathType", (float)DeathType.RED_TILE);
    }

    private void PlayGameOverSFX()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.gameOver, transform);
    }
}
