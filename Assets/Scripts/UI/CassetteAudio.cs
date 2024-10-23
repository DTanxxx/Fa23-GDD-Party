using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Lurkers.Environment.Hearing
{
    public class CassetteAudio : MonoBehaviour
    {
        AudioSource aud;
        [SerializeField] private Button cassetteButton = null;
        [SerializeField] Animator animator;
        [SerializeField] private float waitTime = 1.0f;

        private static bool firstTime = true;

        public static Action onFirstTimePlayCassette;

        private void Start()
        {
            aud = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            cassetteButton.onClick.AddListener(PlayCassette);
        }

        private void OnDisable()
        {
            cassetteButton.onClick.RemoveListener(PlayCassette);
        }

        private void DelayedDialogue()
        {
            onFirstTimePlayCassette?.Invoke();
        }

        private void PlayCassette()
        {
            if (firstTime)
            {
                firstTime = false;
                Invoke("DelayedDialogue", 1.0f);
            }

            aud.loop = false;
            aud.Stop();
            aud.PlayOneShot(aud.clip);
        }

        public void Playing()
        {
            animator.SetBool("ButtonPressed", true);
            Invoke("StopPlay", waitTime);
        }

        void StopPlay()
        {
            animator.SetBool("ButtonPressed", false);
        }
    }
}
