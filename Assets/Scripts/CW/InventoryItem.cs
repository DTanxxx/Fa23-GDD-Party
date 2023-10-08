using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
    public class InventoryItem
{
    public Item data { get; private set; }
    public int stackSize { get; private set; }

    public InventoryItem(Item source)
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
