using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Lurkers.Inventory;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler 
{
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] int inventorySlotIndex;
   
    public void OnDrop(PointerEventData eventData)
    {
        //check if empty:
        if (transform.childCount == 0)
        {
            Debug.Log("dropping on " + gameObject.name);
            GameObject dropped = eventData.pointerDrag;
            ItemClick dragItem = dropped.GetComponent<ItemClick>();
            dragItem.endParent = transform;
        } else
        {
            GameObject dropped = eventData.pointerDrag;
            ItemClick draggableItem = dropped.GetComponent<ItemClick >();

            GameObject current = transform.GetChild(0).gameObject;
            ItemClick currentDraggable = current.GetComponent<ItemClick>();

            currentDraggable.transform.SetParent(draggableItem.endParent);
            draggableItem.endParent = transform;
        }
    }
}
