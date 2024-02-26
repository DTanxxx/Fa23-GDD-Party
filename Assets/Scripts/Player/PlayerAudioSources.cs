using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class PlayerAudioSources : MonoBehaviour
{
    public static EventInstance breathingAndHeartbeat;
    public static EventInstance death;
    private enum Enemy
    {
        WEEPING_ANGEL = 0,
    }

    private enum TileEffect
    {
        BLUE = 1,
        RED = 2,
    }

    private void OnEnable()
    {
        PlayerAnimationEvents.onFootstep += PlayFootstepSFX;
        PlayerAnimationEvents.onSkullCrush += PlaySkullCrushSFX;
        PlayerMovement.onPlayerSlide += PlaySlideSFX;
        PlayerMovement.onPlayerEndSlide += StopSlideSFX;
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
        PlayerMovement.onPlayerSlide -= PlaySlideSFX;
        PlayerMovement.onPlayerEndSlide -= StopSlideSFX;
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

    private void PlaySlideSFX()
    {
        AudioManager.instance.SetPlaySingleton("slide", FMODEvents.instance.tileEffect, transform, "Tile", (float)TileEffect.BLUE);
    }

    private void StopSlideSFX()
    {
        AudioManager.instance.Stop("slide");
    }

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
        AudioManager.instance.SetPlayOneShot(FMODEvents.instance.enemy, transform, "Enemy", (float)Enemy.WEEPING_ANGEL);
    }

    private void PlayIncinerateSFX()
    {
        StopAllSFX();
        AudioManager.instance.SetPlayOneShot(FMODEvents.instance.tile, transform, "Tile", (float)Tile.RED);
    }

    private void PlayGameOverSFX()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.gameOver, transform);
    }
}
