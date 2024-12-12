using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Lurkers.Inventory;
using System;

namespace Lurkers.UI
{
    public class ItemClick : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] int inventorySlotIndex;

        private Transform endParent;
        private Image image;

        public static Action<ItemData> onEnterHover;
        public static Action onExitHover;
        public static Action<ItemData> onClick;
        public static Action onSolutionMix;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (image.sprite == null)
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
                if (targetSlot.image.sprite != null &&
                    targetObject != null)
                {
                    // Swap the inventory items
                    int targetIndex = targetSlot.inventorySlotIndex;

                    ItemData thisData = InventorySystem.Instance.GetIndexInventory(inventorySlotIndex)?.data;
                    ItemData targetData = InventorySystem.Instance.GetIndexInventory(targetIndex)?.data;

                    if (thisData == null || targetData == null)
                    {
                        transform.SetParent(endParent, false);
                        transform.localPosition = Vector3.zero;
                        image.raycastTarget = true;
                        return;
                    }

                    // update inventory
                    if (thisData.Interact(targetData))
                    {
                        InventorySystem.Instance.SetIndexInventory(inventorySlotIndex, thisData);
                        InventorySystem.Instance.SetIndexInventory(targetIndex, targetData);
                        onSolutionMix?.Invoke();
                    }
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            GameObject thisObj = eventData.pointerCurrentRaycast.gameObject;
            if (thisObj.GetComponent<ItemClick>() != null)
            {
                ItemData thisData = InventorySystem.Instance.GetIndexInventory(inventorySlotIndex)?.data;

                if (thisData != null)
                {
                    onEnterHover?.Invoke(thisData);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onExitHover?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameObject thisObj = eventData.pointerCurrentRaycast.gameObject;
            if (thisObj.GetComponent<ItemClick>() != null)
            {
                ItemData thisData = InventorySystem.Instance.GetIndexInventory(inventorySlotIndex)?.data;

                if (thisData != null)
                {
                    onClick?.Invoke(thisData);
                    onEnterHover?.Invoke(thisData);
                }
            }
        }
    }
}
