using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMOD.Studio;
using Lurkers.Camera;
using Lurkers.Environment.Vision;
using Lurkers.Audio;  // TODO this namespace should not be here - it is redundant when we are already using Lurkers.Audio.Player
using Lurkers.Audio.Player;
using Lurkers.Event;
using Lurkers.Vision;

namespace Lurkers.Control
{
    public class EnemyProximitySensor : MonoBehaviour
    {
        [SerializeField] private float enemySensorRadius = 2f;
        [SerializeField] private LayerMask weepingAngelLayer;
        [SerializeField] private LayerMask faceHuggerLayer;

        public static Action onEnemyInProximity;
        public static Action onEnemyOutOfProximity;

        private bool gameStarted = false;

        private void OnEnable()
        {
            ElevatorOpen.onElevatorClose += BeginGame;
        }

        private void OnDisable()
        {
            ElevatorOpen.onElevatorClose -= BeginGame;
        }

        private void BeginGame()
        {
            gameStarted = true;

            // TODO this should be abstracted away in the PlayerAudioSources class
            AudioManager.instance.SetPlaySingleton("breathingAndHeartbeat", FMODEvents.instance.breathingAndHeartbeat, transform, "Intensity", 0f);
            /*PlayerAudioSources.breathingAndHeartbeat = AudioManager.instance.CreateEventInstance(FMODEvents.instance.breathingAndHeartbeat, transform);
            PLAYBACK_STATE playbackState;
            PlayerAudioSources.breathingAndHeartbeat.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED)) PlayerAudioSources.breathingAndHeartbeat.start();*/
        }

        private void Update()
        {
            if (!gameStarted)
            {
                return;
            }

            // create a sphere raycast and check if there are any enemies within the specified radius
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, enemySensorRadius, transform.forward, 0f,
                (weepingAngelLayer | faceHuggerLayer), QueryTriggerInteraction.Ignore);

            // TODO: If enemy in proximity, vary FMOD INTENSITY parameter based on distance between closest enemy and player
            // process each enemy, only detect them if they are active
            bool detected = false;
            foreach (var hit in hits)
            {
                if (hit.transform.GetComponentInParent<IFlashable>().IsActive())
                {
                    // enemy in proximity!
                    onEnemyInProximity?.Invoke();
                    detected = true;
                    break;
                }
            }

            if (!detected)
            {
                // no enemy in proximity
                onEnemyOutOfProximity?.Invoke();
            }
        }
    }
}
