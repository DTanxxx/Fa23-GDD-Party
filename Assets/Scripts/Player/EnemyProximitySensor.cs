using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
