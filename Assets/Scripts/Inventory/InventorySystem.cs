using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Lurkers.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        [SerializeField] private GameObject pickupGUI;

        private Dictionary<ItemData, InventoryItemData> m_itemDictionary;

        public List<InventoryItemData> inventory { get; private set; }

        public static InventorySystem instance;

        private void Awake()
        {
            instance = this;
            inventory = new List<InventoryItemData>();
            m_itemDictionary = new Dictionary<ItemData, InventoryItemData>();
            pickupGUI.SetActive(false);
        }

        public InventoryItemData Get(ItemData referenceData)
        {
            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItemData value))
            {
                return value;
            }
            return null;
        }

        public void Add(ItemData referenceData)
        {
            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItemData value))
            {
                value.AddtoStack();
            }
            else
            {
                InventoryItemData newItem = new InventoryItemData(referenceData);
                inventory.Add(newItem);
                m_itemDictionary.Add(referenceData, newItem);
            }
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
            }
        }

        public void OpenGUI(string input = "Press E to pick up item")
        {
            pickupGUI.GetComponentInChildren<TextMeshProUGUI>().text = input;
            pickupGUI.SetActive(true);
        }

        public void CloseGUI()
        {
            pickupGUI.SetActive(false);
        }
    }
}
