using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

namespace Lurkers.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        [SerializeField] private GameObject hotBar;

        private Dictionary<ItemData, InventoryItemData> m_itemDictionary;

        public List<InventoryItemData> inventory { get; private set; }

        public static InventorySystem instance;


        private void Awake()
        {
            instance = this;
            inventory = new List<InventoryItemData>();
            m_itemDictionary = new Dictionary<ItemData, InventoryItemData>();
        }

        public InventoryItemData Get(ItemData referenceData)
        {
            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItemData value))
            {
                return value;
            }
            return null;
        }

        public void SetIndexInventory(int index, ItemData referenceData)
        {
            InventoryItemData tempData = Get(referenceData);
            if (tempData != null) 
            {
                inventory[index] = tempData;
            }
        }

        public InventoryItemData GetIndexInventory(int index)
        {
            return inventory[index];
        }

        public void Add(ItemData referenceData)
        {
            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItemData value) &&
                referenceData.stackable)
            {
                value.AddtoStack();
            }
            else
            {
                InventoryItemData newItem = new InventoryItemData(referenceData);
                inventory.Add(newItem);
                m_itemDictionary.Add(referenceData, newItem);
            }
            //hotBar.GetComponent<Hotbar>().addItem(referenceData);
        }

        public void Remove(ItemData referenceData)
        {
            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItemData value))
            {
                value.RemoveFromStack();

                if (value.stackSize == 0)
                {
                    inventory.Remove(value);
                    m_itemDictionary.Remove(referenceData);
                }
                //hotBar.GetComponent<Hotbar>().removeItem(referenceData);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < inventory.Count; i++)
            {
                sb.Append("index " + i + " sprite: " + inventory[i].data.icon + "\n");
            }
            return sb.ToString();
        }
    }
}
