using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Inventory
{
    public class DetectItem : MonoBehaviour
    {
        [SerializeField] InventorySystem inventorySystem;
        
        private ItemObject item;

        public static Action onLeaveItem;
        public static Action<Vector3> onApproachItem;

        // Update is called once per frame
        void Update()
        {
            if (item != null)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    item.PickUp();
                    onLeaveItem?.Invoke();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            item = other.gameObject.GetComponent<ItemObject>();
            if (item != null)
            {
                onApproachItem?.Invoke(transform.position);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            item = other.gameObject.GetComponent<ItemObject>();
            if (item != null)
            {
                onLeaveItem?.Invoke();
                item = null;
            }

        }
    }
}
