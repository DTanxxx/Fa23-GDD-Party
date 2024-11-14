using Lurkers.Inventory;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Hotbar : MonoBehaviour{
    [SerializeField] Image[] hotbarSlot = new Image[4];
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite Plank; //will be set to placeholder sprite for now
    [SerializeField] Sprite Rope;  //will be set to placeholder sprite for now
    [SerializeField] ItemData Flask;  //will be set to placeholder sprite for now
    [SerializeField] ItemData Flask2;  //will be set to placeholder sprite for now
    [SerializeField] ItemData Other;  //will be set to placeholder sprite for now
    [SerializeField] InventorySystem inventorySystem;
    private void Start()
    {
        //populating inventory temporarily for testing
        inventorySystem.Add(Flask);
        inventorySystem.Add(Flask2);
        updateHotBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1");
            updateHotBar();
            foreach(Image image in hotbarSlot)
            {
                Color c = image.color;
                if (image.sprite == emptySprite)
                {
                    //will be flask for testing
                    image.sprite = Plank;
                    //image.sprite = Flask; 
                    c.a = 100;
                    image.color = c;
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2");
            foreach(Image image in hotbarSlot)
            {
                Color c = image.color;
                if (image.sprite == emptySprite)
                {
                    image.sprite = Rope;
                    //image.sprite = Flask; 
                    c.a = 100;
                    image.color = c;
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            for (int i = 0; i < hotbarSlot.Length; i++)
            {
                Color c = hotbarSlot[i].color;
                if (hotbarSlot[i].sprite == Plank)
                {
                    hotbarSlot[i].sprite = emptySprite;
                    c.a = 0;
                    hotbarSlot[i].color = c;
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            for (int i = 0; i < hotbarSlot.Length; i++)
            {
                Color c = hotbarSlot[i].color;
                if (hotbarSlot[i].sprite == Rope)
                {
                    hotbarSlot[i].sprite = emptySprite;
                    c.a = 0;
                    hotbarSlot[i].color = c;
                    break;
                }
            }
        }
    }

    public void updateHotBar()
    {
        for (int i = 0; i < hotbarSlot.Length ; i++)
        {
            if (i < inventorySystem.inventory.Count)
            {
                InventoryItemData item = inventorySystem.inventory[i]; 
                hotbarSlot[i].sprite = item.data.icon;
                hotbarSlot[i].color = new Color(1, 1, 1, 1);
            } 
            else 
            { 
                hotbarSlot[i].sprite = emptySprite;
                Color color = hotbarSlot[i].color;
                color.a = 0f;
                hotbarSlot[i].color = color;

            }
        }
    }
}
