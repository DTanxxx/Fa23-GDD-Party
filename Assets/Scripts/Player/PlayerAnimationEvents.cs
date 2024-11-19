using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lurkers.Event
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        [SerializeField] private float pauseBeforeDeathUI = 1f;

        public static Action onFootstep;
        public static Action onEndPlayerDeathAnim;
        public static Action onSkullCrush;
        public static Action onFacehug;
        public static Action onFlashlightBreak;
        public static Action onFinishThrow;

        private WaitForSeconds waitForDeathUI;

        private void Start()
        {
            waitForDeathUI = new WaitForSeconds(pauseBeforeDeathUI);
        }

        // animation event
        public void OnEndDeathAnimation()
        {
            // wait for 1 second
            StartCoroutine(DeathSequence());
        }

        private IEnumerator DeathSequence()
        {
            yield return waitForDeathUI;
            onEndPlayerDeathAnim?.Invoke();
        }

        // animation event
        public void OnSkullCrush()
        {
            onSkullCrush?.Invoke();
        }

        // animation event
        public void OnFacehug()
        {
            onFacehug?.Invoke();
        }

        // animation event
        public void OnFootstep()
        {
            onFootstep?.Invoke();
        }

        // animation event
        public void OnFlashlightBreak()
        {
            onFlashlightBreak?.Invoke();
        }

        // animation event
        public void OnFinishThrow()
        {
            onFinishThrow?.Invoke();
        }
    }
}
