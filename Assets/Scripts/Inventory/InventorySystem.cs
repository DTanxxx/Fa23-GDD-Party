using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Lurkers.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        [SerializeField] private int maxInventorySize = 4;

        private Dictionary<ItemData, InventoryItemData> m_itemDictionary;
        private List<InventoryItemData> inventoryList;  // ordering here is important

        public static InventorySystem Instance;
        public static Action onUpdateInventory;

        private void Awake()
        {
            Instance = this;
            inventoryList = new List<InventoryItemData>();
            m_itemDictionary = new Dictionary<ItemData, InventoryItemData>();
        }

        public void Refresh()
        {
            onUpdateInventory?.Invoke();
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
                inventoryList[index] = tempData;
                onUpdateInventory?.Invoke();
            }
        }

        public InventoryItemData GetIndexInventory(int index)
        {
            if (inventoryList.Count <= index)
            {
                return null;
            }
            return inventoryList[index];
        }

        public List<InventoryItemData> GetInventoryList()
        {
            return inventoryList;
        }

        public bool Add(ItemData referenceData)
        {
            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItemData value) &&
                referenceData.stackable)
            {
                value.AddtoStack();
                onUpdateInventory?.Invoke();
                return true;
            }
            else if (inventoryList.Count < maxInventorySize)
            {
                InventoryItemData newItem = new InventoryItemData(referenceData);
                inventoryList.Add(newItem);
                if (!m_itemDictionary.TryGetValue(referenceData, out InventoryItemData v))
                {
                    m_itemDictionary.Add(referenceData, newItem);
                }
                onUpdateInventory?.Invoke();
                return true;
            }
            return false;
        }

        public void Remove(ItemData referenceData)
        {
            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItemData value))
            {
                value.RemoveFromStack();

                if (value.stackSize == 0)
                {
                    inventoryList.Remove(value);
                    m_itemDictionary.Remove(referenceData);
                }

                onUpdateInventory?.Invoke();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < inventoryList.Count; i++)
            {
                sb.Append("index " + i + " sprite: " + inventoryList[i].data.icon + "\n");
            }
            return sb.ToString();
        }
    }
}
