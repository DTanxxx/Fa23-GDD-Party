using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lurkers.UI.Vision
{
    public class SecurityCamEnabler : MonoBehaviour
    {
        private bool pickedUp = false;

        public static Action onSecurityCamPickup;
        public static Action onFirstTimeCamPickup;

        private void OnTriggerEnter(Collider other)
        {
            GameObject player = other.transform.parent.parent.gameObject;

            if (!player.gameObject.CompareTag("Player"))
            {
                return;
            }

            if (!pickedUp)
            {
                pickedUp = true;
                onSecurityCamPickup?.Invoke();
                onFirstTimeCamPickup?.Invoke();
            }      
        }
    }
}
