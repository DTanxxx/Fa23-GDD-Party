using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LeverPullAnimationEvents : MonoBehaviour
{
    public static Action onBeginLeverCinematicSequence;

    // animation event
    public void BeginCinematicSequence()
    {
        onBeginLeverCinematicSequence?.Invoke();
    }
}
