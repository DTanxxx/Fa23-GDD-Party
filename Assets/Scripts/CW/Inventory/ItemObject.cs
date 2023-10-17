using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] public InventorySystem inventorySystem;
    [SerializeField] private ItemData referenceItem;

    public void PickUp()
    {
        inventorySystem.Add(referenceItem);
        Destroy(gameObject);
    }
}
