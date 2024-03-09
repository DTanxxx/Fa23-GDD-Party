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
        }

        private void OnDisable()
        {
            LevelManager.onPauseGame -= PauseAllSFX;
        }

        private void PauseAllSFX(bool toPause)
        {
            if (toPause)
            {
                raiseSource.Pause();
            }
            else
            {
                raiseSource.UnPause();
            }
        }

        public void PlayRaiseSFX()
        {
            raiseSource.PlayOneShot(raiseSource.clip);
        }
    }
}
