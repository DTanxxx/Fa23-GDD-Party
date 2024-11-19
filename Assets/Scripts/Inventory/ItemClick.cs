using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Lurkers.Inventory;

public class ItemClick : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Hotbar theHotbar;
    [SerializeField] InventorySystem invSys;
    [SerializeField] int inventorySlotIndex;
    public Image image;
    [HideInInspector] public Transform endParent;
    [SerializeField] Sprite emptySprite;

    public void OnBeginDrag(PointerEventData eventData) 
    {
        if (image.sprite == emptySprite)
        {
            return;
        }
        endParent = transform.parent;
        transform.SetParent(transform.root.GetComponentInChildren<Canvas>().transform, false);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    public void OnEndDrag(PointerEventData eventData) 
    {
        Debug.Log("up");
        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;
        if (targetObject != null && targetObject.GetComponent<ItemClick>() != null)
        {
            ItemClick targetSlot = targetObject.GetComponent<ItemClick>();

            // Swap the inventory items
            int targetIndex = targetSlot.inventorySlotIndex;

            ItemData thisData = invSys.GetIndexInventory(inventorySlotIndex).data;
            ItemData targetData = invSys.GetIndexInventory(targetIndex).data;

            thisData.Interact(targetData);

            //swap inventory index
            invSys.SetIndexInventory(targetSlot.inventorySlotIndex, thisData);
            invSys.SetIndexInventory(inventorySlotIndex, targetData);

            //swap inherent index
            targetSlot.inventorySlotIndex = inventorySlotIndex;
            inventorySlotIndex = targetIndex;

            theHotbar.updateHotBar();
        }
        transform.SetParent(endParent, false);
        transform.localPosition = Vector3.zero;
        image.raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData) 
    {
        transform.position = Input.mousePosition;
    }
}
