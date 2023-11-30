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

    private bool sensorEnabled = false;

    private void OnEnable()
    {
        CameraFollow.onCameraRestoreComplete += EnableSensor;
    }

    private void OnDisable()
    {
        CameraFollow.onCameraRestoreComplete -= EnableSensor;
    }

    private void EnableSensor()
    {
        sensorEnabled = true;
    }

    private void Update()
    {
        if (!sensorEnabled)
        {
            return;
        }

        // create a sphere raycast and check if there are any enemies within the specified radius
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, enemySensorRadius, transform.forward, 0f,
            weepingAngelLayer.value, QueryTriggerInteraction.Ignore);
        
        if (hits.Length > 0)
        {
            // enemy in proximity!
            Debug.Log("DETECTED");
            onEnemyInProximity?.Invoke();
        }
        else
        {
            // no enemy in proximity
            Debug.Log("NO DETECTION");
            onEnemyOutOfProximity?.Invoke();
        }
    }
}
