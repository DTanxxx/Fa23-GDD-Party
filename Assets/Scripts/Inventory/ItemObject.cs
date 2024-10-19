using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Inventory
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] public InventorySystem inventorySystem;
        [SerializeField] private ItemData referenceItem;
        [SerializeField] Hotbar hb;

        public void PickUp()
        {
            inventorySystem.Add(referenceItem);
            hb.addItem(referenceItem.icon);
            Destroy(gameObject);
        }

        public ItemData GetItemData()
        {
            return referenceItem;
        }
    }
}
