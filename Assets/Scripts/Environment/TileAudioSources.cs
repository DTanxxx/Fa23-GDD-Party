using Lurkers.Control.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Audio.Vision
{
    public class TileAudioSources : MonoBehaviour
    {
        [SerializeField] private AudioSource raiseSource = null;

        private void OnEnable()
        {
            LevelManager.onPauseGame += PauseAllSFX;
            LevelManager.onUnpausegame += UnpauseAllSFX;
        }

        private void OnDisable()
        {
            LevelManager.onPauseGame -= PauseAllSFX;
            LevelManager.onUnpausegame -= UnpauseAllSFX;
        }

        private void PauseAllSFX()
        {
            raiseSource.Pause();
        }

        private void UnpauseAllSFX()
        {
            raiseSource.UnPause();
        }

        public void PlayRaiseSFX()
        {
            raiseSource.PlayOneShot(raiseSource.clip);
        }
    }
}
