using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lurkers.Audio;
using Lurkers.Inventory;  // TODO pulling lever should not depend on inventory

namespace Lurkers.Environment.Vision
{
    public class PullLever : MonoBehaviour
    {
        [SerializeField] bool triggerLightFlicker = false;
        [SerializeField] bool lockable = false;
        [SerializeField] InventorySystem inventorySystem;
        [SerializeField] private Animator animator;

        public static Action onLeverPulled;

        private GameObject item;
        private bool pulled = false;
        private bool locked;

        private void Awake()
        {
            locked = lockable;
            inventorySystem = FindObjectOfType<InventorySystem>();
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
            if (item != null && !pulled && !locked)
            {
                if (Input.GetKeyDown(KeyCode.E))
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

            item = other.gameObject;

            if (other != null)
            {
                if (locked)
                {
                    inventorySystem.OpenGUI("Lever is locked! Search for the key in a vault somewhere...");
                }
                else
                {
                    inventorySystem.OpenGUI("Press E to pull lever");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != null)
            {
                inventorySystem.CloseGUI();
                item = null;
            }
        }

        private void LeverPulled()
        {
            onLeverPulled?.Invoke();

            if (animator != null)
            {
                pulled = true;
                inventorySystem.CloseGUI();
                item = null;

                animator.SetTrigger("Pulled");
                AudioManager.instance.PlayOneShot(FMODEvents.instance.lever, transform);
            }
            else
            {
                Debug.LogError("One of the fields is null!");
            }
        }
    }
}
