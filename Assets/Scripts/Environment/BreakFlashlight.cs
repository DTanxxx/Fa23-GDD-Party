using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lurkers.Environment.Hearing
{
    public class BreakFlashlight : MonoBehaviour
    {
        private bool ended = false;

        public static Action onBreakFlashlight;

        private void OnTriggerEnter(Collider other)
        {
            if (!ended)
            {
                ended = true;
                onBreakFlashlight?.Invoke();
            }
        }
    }
}
