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
   
    public void OnDrop(PointerEventData eventData)
    {
            //item being dragged
            GameObject dropped = eventData.pointerDrag;
            ItemClick draggableItem = dropped.GetComponent<ItemClick>();

            //item being dropped onto
            GameObject current = transform.GetChild(0).gameObject;
            ItemClick currentDraggable = current.GetComponent<ItemClick>();

            currentDraggable.transform.SetParent(draggableItem.endParent);
            draggableItem.endParent = transform;
    }
}
