using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lurkers.Environment.Vision
{
    public class FlickerTrigger : MonoBehaviour
    {
        private bool flickering = false;

        private static bool firstTime = true;

        public static Action onFlashlightFlicker;
        public static Action onFlashlightFlickerFirstTime;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // light begins to flicker
                if (!flickering)
                {
                    flickering = true;
                    onFlashlightFlicker?.Invoke();

                    if (firstTime)
                    {
                        firstTime = false;
                        onFlashlightFlickerFirstTime?.Invoke();
                    }
                }
            }
        }
    }
}
