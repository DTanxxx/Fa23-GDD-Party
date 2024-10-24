using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Inventory;

public class IngredientSupply : MonoBehaviour
{
    [SerializeField] public InventorySystem inventorySystem;
    [SerializeField] private Hotbar hb;
    [SerializeField] private ItemData filledFlask;
    private bool interacting = false;
    
    
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
            inventorySystem.OpenGUI("Press Q to fill up flask");
            interacting = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            inventorySystem.CloseGUI();
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

    

    [CreateAssetMenu]
    public class Flavor : ScriptableObject
    {
        [Range(0f, 1f)]
        public float sweet;

        [Range(0f, 1f)]
        public float bitter;

        [Range(0f, 1f)]
        public float sour;

        [Range(0f, 1f)]
        public float salty;

        [Range(0f, 1f)]
        public float umami;
    }


    public class Formula : MonoBehaviour
    {
        public static Flavor Combine(Flavor A, Flavor B)
        {
            Flavor newFlavor = ScriptableObject.CreateInstance<Flavor>();
            newFlavor.sweet = (A.sweet + B.sweet) / 2;
            newFlavor.bitter = (A.bitter + B.bitter) / 2;
            newFlavor.salty = (A.salty + B.salty) / 2;
            newFlavor.sour = (A.sour + B.sour) / 2;
            newFlavor.umami = (A.umami + B.umami) / 2;
            return newFlavor;
            
        }
    }
}
