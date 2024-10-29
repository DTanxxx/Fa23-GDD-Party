using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Inventory;
using System;

public class IngredientSupply : MonoBehaviour
{
    [SerializeField] public InventorySystem inventorySystem;
    [SerializeField] private Hotbar hb;
    [SerializeField] private ItemData filledFlask;
    
    private bool interacting = false;

    public static Action onLeaveStation;
    public static Action<Vector3> onApproachStation;
    
    private void Update()
    {
        if (interacting)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (hb.hbContains("1"))
                {
                    fillFlask();
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onApproachStation?.Invoke(transform.position);
            interacting = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            onLeaveStation?.Invoke();
            interacting = false;
        }
    }

    private void fillFlask()
    {
        ItemData emptyFlask = hb.getItem("1");
        hb.removeItem(emptyFlask);
        inventorySystem.Remove(emptyFlask);
        hb.addItem(filledFlask);
        inventorySystem.Add(filledFlask);
        Flavor newflav = ScriptableObject.CreateInstance<Flavor>();
        newflav.sweet = 1;
        newflav.salty = 0;
        newflav.bitter = 0;
        newflav.sour = 0;
        newflav.umami = 0;
        
    }
}
