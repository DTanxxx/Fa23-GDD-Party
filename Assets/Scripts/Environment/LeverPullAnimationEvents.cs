using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LeverPullAnimationEvents : MonoBehaviour
{
    [SerializeField] private bool animateLightFlicker = false;

    public static Action onBeginLeverCinematicSequence;

    // animation event
    public void BeginCinematicSequence()
    {
        if (animateLightFlicker)
        {
            onBeginLeverCinematicSequence?.Invoke();
        }
    }
}
