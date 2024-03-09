using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Lurkers.Inventory;
using Lurkers.Camera;
using Lurkers.Control.Vision.Character;

namespace Lurkers.Control
{
    public class EnemyActivate : MonoBehaviour
    {
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private ItemObject activationItem;
        [SerializeField] private WeepingAngelMovement[] weepingAngels;

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
        }

        private void OnDisable()
        {
            CameraFollow.onCameraRestoreComplete -= SetActive;
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
    }
}
