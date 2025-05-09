using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Lurkers.Inventory;
using Lurkers.Cam;
using Lurkers.Event;
using Lurkers.Control.Vision;
using Lurkers.Environment.Vision;

namespace Lurkers.Control
{
    public class EnemyActivate : MonoBehaviour
    {
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private ItemObject activationItem;
        [SerializeField] private WeepingAngelController[] weepingAngels;

        //private ItemData itemData;
        private bool active = false;

        private void Awake()
        {
            /*if (activationItem != null)
            {
                itemData = activationItem.GetItemData();
            }*/
        }

        private void OnEnable()
        {
            CameraFollow.onCameraRestoreComplete += SetActive;
            LeverPullAnimationEvents.onLeverPullNoFlicker += SetActive;
            Vault.onVaultOpened += SetActive;
        }

        private void OnDisable()
        {
            CameraFollow.onCameraRestoreComplete -= SetActive;
            LeverPullAnimationEvents.onLeverPullNoFlicker -= SetActive;
            Vault.onVaultOpened -= SetActive;
        }

        private void Update()
        {
            /*if (active || activationItem == null)
            {
                return;
            }

            if (inventorySystem.Get(itemData) != null)
            {
                if (inventorySystem.Get(itemData).stackSize == 1)
                {
                    SetActive();
                }
            }*/
        }

        public void SetActive()
        {
            foreach (var enemy in weepingAngels)
            {
                if (enemy != null)
                {
                    Debug.Log(enemy.gameObject.name + " is active!");
                    enemy.gameObject.SetActive(true);
                    enemy.SetActive();
                    active = true;
                }
            }
        }

        public bool IsActive()
        {
            return active;
        }
    }
}
