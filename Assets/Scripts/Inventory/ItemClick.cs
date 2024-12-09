using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Lurkers.Inventory;
using Lurkers.Audio;

public class ItemClick : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Hotbar theHotbar;
    [SerializeField] InventorySystem invSys;
    public int inventorySlotIndex;
    [HideInInspector] public Transform endParent;
    [SerializeField] Sprite emptySprite;
    [SerializeField] AudioSource uiSound;
    public Image image;

    public void OnBeginDrag(PointerEventData eventData) 
    {
        Debug.Log("up");
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
        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;
        if (targetObject.GetComponent<ItemClick>() != null)
        {
            ItemClick targetSlot = targetObject.GetComponent<ItemClick>();
            if (targetSlot.image.sprite != emptySprite &&
                targetObject != null)
            {
                // Swap the inventory items
                int targetIndex = targetSlot.inventorySlotIndex;

                ItemData thisData = invSys.GetIndexInventory(inventorySlotIndex).data;
                ItemData targetData = invSys.GetIndexInventory(targetIndex).data;

                if (thisData.Interact(targetData))
                {
                    theHotbar.updateHotBar(inventorySlotIndex, thisData);
                    theHotbar.updateHotBar(targetIndex, targetData);
                    uiSound.PlayOneShot(thisData.itemSFX);
                    //AudioManager.instance.PlayOneShot(thisData.itemSFX, transform);
                }

                //swap inherent index
                //invSys.SetIndexInventory(inventorySlotIndex, targetData); 
                //invSys.SetIndexInventory(targetIndex, thisData); 
                //targetSlot.inventorySlotIndex = inventorySlotIndex;
                //inventorySlotIndex = targetIndex;
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
}
