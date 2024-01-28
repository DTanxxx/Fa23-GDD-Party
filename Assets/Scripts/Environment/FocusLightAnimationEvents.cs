using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FocusLightAnimationEvents : MonoBehaviour
{
    [SerializeField] private Transform enemyUnderFocus = null;
    [SerializeField] private Transform teleportDestination = null;

    public static Action onFocusLightFinishAnim;

    // animation event
    public void TeleportEnemy()
    {
        enemyUnderFocus.gameObject.SetActive(false);
        enemyUnderFocus.position = teleportDestination.position;
    }

    // animation event
    public void FinishFlicker()
    {
        onFocusLightFinishAnim?.Invoke();
    }
}
