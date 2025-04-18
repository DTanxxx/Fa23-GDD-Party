using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using Lurkers.Event;
using Lurkers.Environment.Vision;
using Lurkers.Control;
using Lurkers.Control.Level;
using Lurkers.Cam;

namespace Lurkers.Audio.Player
{
    public class PlayerAudioSources : MonoBehaviour
    {
        private enum TileEffect
        {
            BLUE = 0,
            RED = 1,
        }

        private void OnEnable()
        {
            PlayerAnimationEvents.onFootstep += PlayFootstepSFX;
            PlayerAnimationEvents.onSkullCrush += PlaySkullCrushSFX;
            PlayerAnimationEvents.onFacehug += PlayFacehugSFX;
            PlayerController.onPlayerSlide += PlaySlideSFX;
            PlayerController.onPlayerEndSlide += StopSlideSFX;
            PlayerAnimationEvents.onEndPlayerDeathAnim += PlayGameOverSFX;
            EnemyProximitySensor.onEnemyInProximity += EnterPanicPhase;
            EnemyProximitySensor.onEnemyOutOfProximity += EnterCalmPhase;
            NextLevelTrigger.onBeginLevelTransition += StopAllSFX;
            ElevatorOpen.onElevatorClose += EnterCalmPhase;
            LevelManager.onPauseGame += PauseAllSFX;
            LevelManager.onUnpausegame += UnpauseAllSFX;
            PullLever.onLeverPulled += PauseAllSFX;
            CameraFollow.onCameraRestoreComplete += UnpauseAllSFX;
            ColorTile.onIncinerate += PlayIncinerateSFX;
        }

        private void OnDisable()
        {
            PlayerAnimationEvents.onFootstep -= PlayFootstepSFX;
            PlayerAnimationEvents.onSkullCrush -= PlaySkullCrushSFX;
            PlayerAnimationEvents.onFacehug -= PlayFacehugSFX;
            PlayerController.onPlayerSlide -= PlaySlideSFX;
            PlayerController.onPlayerEndSlide -= StopSlideSFX;
            PlayerAnimationEvents.onEndPlayerDeathAnim -= PlayGameOverSFX;
            EnemyProximitySensor.onEnemyInProximity -= EnterPanicPhase;
            EnemyProximitySensor.onEnemyOutOfProximity -= EnterCalmPhase;
            NextLevelTrigger.onBeginLevelTransition -= StopAllSFX;
            ElevatorOpen.onElevatorClose -= EnterCalmPhase;
            LevelManager.onPauseGame -= PauseAllSFX;
            LevelManager.onUnpausegame -= UnpauseAllSFX;
            PullLever.onLeverPulled -= PauseAllSFX;
            CameraFollow.onCameraRestoreComplete -= UnpauseAllSFX;
            ColorTile.onIncinerate -= PlayIncinerateSFX;
        }

        private void PauseAllSFX()
        {
            AudioManager.instance.PauseAll();
        }

        private void UnpauseAllSFX()
        {
            AudioManager.instance.UnpauseAll();
        }

        private void PlaySlideSFX()
        {
            AudioManager.instance.SetPlaySingleton("slide", FMODEvents.instance.tileEffect, transform, "TileEffect", (float)TileEffect.BLUE);
        }

        private void StopSlideSFX()
        {
            AudioManager.instance.StopSingleton("slide");
        }

        private void StopAllSFX()
        {
            AudioManager.instance.StopAll();
        }

        private void EnterPanicPhase()
        {
            AudioManager.instance.SetParameterSingleton("breathingAndHeartbeat", "Intensity", 1f);
        }

        private void EnterCalmPhase()
        {
            AudioManager.instance.SetParameterSingleton("breathingAndHeartbeat", "Intensity", 0f);
        }

        private void PlayFootstepSFX()
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.footsteps, transform);
        }

        private void PlaySkullCrushSFX()
        {
            StopAllSFX();
            AudioManager.instance.SetPlayOneShot(FMODEvents.instance.enemy, transform, "Enemy", (float)Enemy.EnemyAudioSources.Enemy.DEATH_BY_WEEPING_ANGEL);
        }

        private void PlayFacehugSFX()
        {
            StopAllSFX();
            AudioManager.instance.SetPlayOneShot(FMODEvents.instance.enemy, transform, "Enemy", (float)Enemy.EnemyAudioSources.Enemy.DEATH_BY_FACEHUGGER);
        }

        private void PlayIncinerateSFX()
        {
            StopAllSFX();
            AudioManager.instance.SetPlayOneShot(FMODEvents.instance.tileEffect, transform, "TileEffect", (float)TileEffect.RED);
        }

        private void PlayGameOverSFX()
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.gameOver, transform);
        }
    }
}
