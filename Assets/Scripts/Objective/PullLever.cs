using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lurkers.Audio;

namespace Lurkers.Environment.Vision
{
    public class PullLever : MonoBehaviour
    {
        [SerializeField] bool triggerLightFlicker = false;
        [SerializeField] bool lockable = false;
        [SerializeField] private Animator animator;

        public static Action onLeverPulled;
        public static Action<bool, Vector3> onApproachLever;
        public static Action onLeaveLever;

        private bool pulled = false;
        private bool locked;

        private void Awake()
        {
            locked = lockable;
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            Vault.onVaultOpened += ObtainKey;
        }

        private void OnDisable()
        {
            Vault.onVaultOpened -= ObtainKey;
        }

        private void Update()
        {
            if (!pulled && !locked)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    LeverPulled();
                }
            }
        }

        private void ObtainKey()
        {
            locked = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (pulled || !other.gameObject.CompareTag("Player"))
            {
                return;
            }

            onApproachLever?.Invoke(!locked, transform.position);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != null)
            {
                onLeaveLever?.Invoke();
            }
        }

        private void LeverPulled()
        {
            onLeverPulled?.Invoke();

            if (animator != null)
            {
                pulled = true;
                onLeaveLever?.Invoke();

                animator.SetTrigger("Pulled");
                AudioManager.instance.PlayOneShot(FMODEvents.instance.lever, transform);
            }
        }
    }
}
