using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lurkers.Audio;
using Lurkers.Inventory;  // TODO opening vault should not depend on inventory

namespace Lurkers.Environment.Vision
{
    public class Vault : MonoBehaviour
    {
        [SerializeField] InventorySystem inventorySystem;
        [SerializeField] private Animator animator;

        public static Action onVaultOpened;
        public static Action onFirstTimeOpen;

        private GameObject item;
        private bool opened = false;

        private static bool firstTime = true;

        private void Awake()
        {
            inventorySystem = FindObjectOfType<InventorySystem>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (item != null && !opened)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenVault();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (opened || !other.gameObject.CompareTag("Player"))
            {
                return;
            }

            item = other.gameObject;

            if (other != null)
            {
                inventorySystem.OpenGUI("Press E to open vault and grab key");
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

        private void OpenVault()
        {
            onVaultOpened?.Invoke();

            if (firstTime)
            {
                firstTime = false;
                onFirstTimeOpen?.Invoke();
            }

            if (animator != null)
            {
                opened = true;
                inventorySystem.CloseGUI();
                item = null;

                // TODO need to refactor this line
                Invoke("ShowConfirmation", 1.0f);

                animator.SetTrigger("Open");
                // TODO need audio here for vault opening
            }
            else
            {
                Debug.LogError("One of the fields is null!");
            }
        }

        private void ShowConfirmation()
        {
            inventorySystem.OpenGUI("Key obtained!");
            Invoke("CloseGUI", 1.0f);
        }

        private void CloseGUI()
        {
            inventorySystem.CloseGUI();
        }
    }
}
