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
        Debug.Log("down");
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
            //targetSlot.inventorySlotIndex = inventorySlotIndex;
            //invSys.inventory[].data = 
            //inventorySlotIndex = targetIndex;

            ItemData thisData = invSys.GetIndexInventory(inventorySlotIndex).data;
            ItemData targetData = invSys.GetIndexInventory(targetIndex).data;
            if (thisData is Flask thisFlask &&
                targetData is Flask targetFlask &&
                thisFlask.full && targetFlask.full)
            {
                Debug.Log("in " + thisFlask.getFlavor().bitter);
                thisFlask.mergeFlask(targetFlask);
                Debug.Log(thisFlask.getFlavor().bitter);
                //temp
                //image.sprite = thisFlask.GetEmpty();
                targetSlot.image.sprite = thisFlask.GetEmpty();

                //update invSys
                //theHotbar.updateHotBar();
            }
        }
        transform.SetParent(endParent, false);
        transform.localPosition = Vector3.zero;
        image.raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData) 
    {
        transform.position = Input.mousePosition;
    }

    public void UpdateInvSys()
    {
        for (int i = 0; i < invSys.inventory.Count; i++) 
        {

        }
    }
}
