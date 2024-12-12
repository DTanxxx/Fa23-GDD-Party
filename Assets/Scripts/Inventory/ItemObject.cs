using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Inventory
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private ItemData pickupItem;

        public static Action onPickup;
        
        public void PickUp()
        {
            if (InventorySystem.Instance.Add(Instantiate(pickupItem)))
            {
                onPickup?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
