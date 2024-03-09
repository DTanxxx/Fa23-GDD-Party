using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lurkers.Event
{
    public class LeverPullAnimationEvents : MonoBehaviour
    {
        [SerializeField] private bool animateLightFlicker = false;

        public static Action onBeginLeverCinematicSequence;
        public static Action onLeverPullNoFlicker;

        // animation event
        public void BeginCinematicSequence()
        {
            if (animateLightFlicker)
            {
                onBeginLeverCinematicSequence?.Invoke();
            }
            else
            {
                onLeverPullNoFlicker?.Invoke();
            }
        }
    }
}
