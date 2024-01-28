using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySensorUI : MonoBehaviour
{
    private Image sensorUI;

    private void Start()
    {
        sensorUI = GetComponent<Image>();
    }

    private void OnEnable()
    {
        EnemyProximitySensor.onEnemyInProximity += ShowUI;
        EnemyProximitySensor.onEnemyOutOfProximity += HideUI;
    }

    private void OnDisable()
    {
        EnemyProximitySensor.onEnemyInProximity -= ShowUI;
        EnemyProximitySensor.onEnemyOutOfProximity -= HideUI;
    }

    private void ShowUI()
    {
        sensorUI.enabled = true;
    }

    private void HideUI()
    {
        sensorUI.enabled = false;
    }
}
