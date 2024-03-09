using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Inventory;

namespace Lurkers.Objective
{
    public class ItemCheck : MonoBehaviour
    {
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private ItemData requiredItem;

        private void OnCollisionEnter(Collision collision)
        {
            if (inventorySystem.Get(requiredItem) != null)
            {
                if (inventorySystem.Get(requiredItem).stackSize == 1)
                {
                    Debug.Log("you have the item");
                }
            }
            else
            {
                Debug.Log("come back with item");
            }
        }
    }
}