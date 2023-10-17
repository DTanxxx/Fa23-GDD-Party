using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class InventoryItemData
{
    public ItemData data { get; private set; }
    public int stackSize { get; private set; }

    public InventoryItemData(ItemData source)
    {
        data = source;
        AddtoStack();
    }

    public void AddtoStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}
