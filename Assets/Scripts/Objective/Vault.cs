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
        [SerializeField] private Animator animator;

        public static Action onVaultOpened;
        public static Action onFirstTimeOpen;
        public static Action<Vector3> onApproachVault;
        public static Action onLeaveVault;

        private GameObject item;
        private bool opened = false;

        private static bool firstTime = true;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (item != null && !opened)
            {
                if (Input.GetKeyDown(KeyCode.Q))
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

            onApproachVault?.Invoke(transform.position);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != null)
            {
                onLeaveVault?.Invoke();
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
                onLeaveVault?.Invoke();

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
            Invoke("CloseGUI", 1.0f);
        }

        private void CloseGUI()
        {
            onLeaveVault?.Invoke();
        }
    }
}
