using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Environment.Vision
{
    public class ColorTileDialogueTrigger : MonoBehaviour
    {
        private bool firstTime = true;

        public static Action onColorTileIntro;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // trigger dialogue
                if (firstTime)
                {
                    firstTime = false;
                    onColorTileIntro?.Invoke();
                }
            }
        }
    }
}
