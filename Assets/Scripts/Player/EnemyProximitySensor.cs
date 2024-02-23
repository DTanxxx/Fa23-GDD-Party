using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMOD.Studio;

public class EnemyProximitySensor : MonoBehaviour
{
    [SerializeField] private float enemySensorRadius = 2f;
    [SerializeField] private LayerMask weepingAngelLayer;

    public static Action onEnemyInProximity;
    public static Action onEnemyOutOfProximity;

    private bool gameStarted = false;
    private bool sensorEnabled = false;

    private void OnEnable()
    {
        CameraFollow.onCameraRestoreComplete += EnableSensor;
        ElevatorOpen.onElevatorClose += BeginGame;
    }

    private void OnDisable()
    {
        CameraFollow.onCameraRestoreComplete -= EnableSensor;
        ElevatorOpen.onElevatorClose -= BeginGame;
    }

    private void EnableSensor()
    {
        sensorEnabled = true;
    }

    private void BeginGame()
    {
        gameStarted = true;

        PlayerAudioSources.breathingAndHeartbeat = AudioManager.instance.CreateEventInstance(FMODEvents.instance.breathingAndHeartbeat, transform);
        PLAYBACK_STATE playbackState;
        PlayerAudioSources.breathingAndHeartbeat.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED)) PlayerAudioSources.breathingAndHeartbeat.start();
    }

    private void Update()
    {
        if (!gameStarted)
        {
            return;
        }

        // create a sphere raycast and check if there are any enemies within the specified radius
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, enemySensorRadius, transform.forward, 0f,
            weepingAngelLayer.value, QueryTriggerInteraction.Ignore);
        
        // TODO: If enemy in proximity, vary FMOD INTENSITY parameter based on distance between closest enemy and player
        if (hits.Length > 0 && sensorEnabled)
        {
            // enemy in proximity!
            //Debug.Log("DETECTED");
            onEnemyInProximity?.Invoke();
        }
        else
        {
            // no enemy in proximity
            //Debug.Log("NO DETECTION");
            onEnemyOutOfProximity?.Invoke();
        }
    }
}
