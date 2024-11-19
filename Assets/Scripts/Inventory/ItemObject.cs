using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Inventory
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] public InventorySystem inventorySystem;
        [SerializeField] private ItemData referenceItem;
        [SerializeField] private Hotbar hb;

        public void PickUp()
        {
            inventorySystem.Add(referenceItem);
            Destroy(gameObject);
        }

        public ItemData GetItemData()
        {
            return referenceItem;
        }
    }
}
