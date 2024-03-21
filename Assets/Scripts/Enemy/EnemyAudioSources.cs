using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Event;

namespace Lurkers.Audio.Enemy
{
    public class EnemyAudioSources : MonoBehaviour
    {
        public enum Enemy
        {
            DEATH_BY_WEEPING_ANGEL = 0, // Implemented, integrated, needs work
            FACEHUGGER_RUN = 1,         // Implemented, integrated, needs work
            FACEHUGGER_KILL = 2,        // Implemented, integrated, needs work
            DEATH_BY_FACEHUGGER = 3,    // Implemented, integrated, needs work
        }

        private void OnEnable()
        {
            EnemyAnimationEvents.onFacehuggerFootstep += PlayFacehuggerFootstepSFX;
        }

        private void OnDisable()
        {
            EnemyAnimationEvents.onFacehuggerFootstep -= PlayFacehuggerFootstepSFX;
        }

        private void PlayFacehuggerFootstepSFX()
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.footsteps, transform);
        }
    }
}

