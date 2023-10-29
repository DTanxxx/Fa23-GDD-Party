using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DetectItem : MonoBehaviour
{
    [SerializeField] InventorySystem inventorySystem;
    private ItemObject item;

    // Update is called once per frame
    void Update()
    {
        if (item != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                item.PickUp();
                inventorySystem.CloseGUI();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        item = collision.gameObject.GetComponent<ItemObject>();
        if (item != null)
        {
            Debug.Log("touch");
            inventorySystem.OpenGUI();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        item = collision.gameObject.GetComponent<ItemObject>();
        if (item != null)
        {
            Debug.Log("not");
            inventorySystem.CloseGUI();
            item = null;
        }
        
    }
}
