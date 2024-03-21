using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lurkers.Audio;
using Lurkers.Inventory;  // TODO pulling lever should not depend on inventory
using Lurkers.UI;
using Lurkers.Control;

namespace Lurkers.Environment.Vision
{
    // TODO refactor needed
    public class PullLever : MonoBehaviour
    {
        [SerializeField] bool triggerLightFlicker = false;
        [SerializeField] InventorySystem inventorySystem;
        [SerializeField] private Animator animator;

        public static Action onLeverPulled;

        private GameObject item;
        private bool pulled = false;
        public Dialogue Monologue;

        private void Awake()
        {
            inventorySystem = FindObjectOfType<InventorySystem>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (item != null && !pulled)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    LeverPulled();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (pulled)
            {
                return;
            }

            item = other.gameObject;

            if (other != null)
            {
                inventorySystem.OpenGUI("Press E to pull lever");
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
            
            FindObjectOfType<EnemyActivate>().SetActive();

            string[] lines = new string[3];
            lines[0] = "Phew. Now that’s done.";
            lines[1] = "Did that statue.. Just move?";
            lines[2] = "Ahh, they’re alive!";
            Monologue.lines = lines;
            Monologue.gameObject.SetActive(true);
        }
    }
}
