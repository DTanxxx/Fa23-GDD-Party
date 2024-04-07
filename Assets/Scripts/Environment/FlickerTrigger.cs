using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lurkers.Environment.Vision
{
    public class FlickerTrigger : MonoBehaviour
    {
        private bool flickering = false;

        public static Action onFlashlightFlicker;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // light begins to flicker
                if (!flickering)
                {
                    flickering = true;
                    onFlashlightFlicker?.Invoke();
                }
            }
        }
    }
}
