using Lurkers.Inventory;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Hotbar : MonoBehaviour
{
    private int hotbarslots = 4;
    [SerializeField] Image[] hotbarSlot = new Image[4];
    [SerializeField] Sprite emptySprite;
    private string[] itemIDs = new string[4];
    private ItemData[] items = new ItemData[4];
    [SerializeField] Sprite Plank; //will be set to placeholder sprite for now
    [SerializeField] Sprite Rope;  //will be set to placeholder sprite for now
    [SerializeField] ItemData flaskItem;  //will be set to placeholder sprite for now
    [SerializeField] ItemData flaskItem2;  //will be set to placeholder sprite for now
    [SerializeField] ItemData Other;  //will be set to placeholder sprite for now
    [SerializeField] InventorySystem inventorySystem;
    private void Start()
    {
        //populating inventory temporarily for testing
        inventorySystem.Add(flaskItem);
        inventorySystem.Add(flaskItem2);
        Debug.Log(inventorySystem.inventory.Count);
        updateHotBar();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     foreach(Image image in hotbarSlot)
        //     {
        //         Color c = image.color;
        //         if (image.sprite == emptySprite)
        //         {
        //             image.sprite = Plank;
        //             c.a = 100;
        //             image.color = c;
        //             break;
        //         }
        //     }
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     foreach(Image image in hotbarSlot)
        //     {
        //         Color c = image.color;
        //         if (image.sprite == emptySprite)
        //         {
        //             image.sprite = Rope;
        //             c.a = 100;
        //             image.color = c;
        //             break;
        //         }
        //     }
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     for (int i = 0; i < hotbarSlot.Length; i++)
        //     {
        //         Color c = hotbarSlot[i].color;
        //         if (hotbarSlot[i].sprite == Plank)
        //         {
        //             hotbarSlot[i].sprite = emptySprite;
        //             c.a = 0;
        //             hotbarSlot[i].color = c;
        //             break;
        //         }
        //     }
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha4))
        // {
        //     for (int i = 0; i < hotbarSlot.Length; i++)
        //     {
        //         Color c = hotbarSlot[i].color;
        //         if (hotbarSlot[i].sprite == Rope)
        //         {
        //             hotbarSlot[i].sprite = emptySprite;
        //             c.a = 0;
        //             hotbarSlot[i].color = c;
        //             break;
        //         }
        //     }
        // }
    }


    public void addItem(ItemData itemToAdd)
    {
        int counter = 0;
        foreach (Image image in hotbarSlot)
        {
            Color c = image.color;
            if (image.sprite == emptySprite)
            {
                image.sprite = itemToAdd.icon;
                itemIDs[counter] = itemToAdd.id;
                items[counter] = itemToAdd;
                c.a = 100;
                image.color = c;
                break;
            }
            counter++;
        }
    }

    public void removeItem(ItemData itemToDelete)
    {
        for (int i = 0; i < hotbarSlot.Length; i++)
        {
            Color c = hotbarSlot[i].color;
            if (hotbarSlot[i].sprite == itemToDelete.icon)
            {
                hotbarSlot[i].sprite = emptySprite;
                itemIDs[i] = null;
                items[i] = itemToDelete;
                c.a = 0;
                hotbarSlot[i].color = c;
                break;
            }
        }
    }

    public bool hbContains(string itemID)
    {
        if (itemIDs.Contains(itemID))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public ItemData getItem(string itemID)
    {
        for (int i = 0; i < hotbarSlot.Length; i++)
        {
            if (itemIDs[i] == itemID)
            {
                return items[i];
            }
            //Debug.Log("upd " + i + " " + hotbarSlot[i].sprite);
        }
        return null;
    }
    public void updateHotBar()
    {
        for (int i = 0; i < hotbarSlot.Length; i++)
        {
            if (i < inventorySystem.inventory.Count)
            {
                InventoryItemData item = inventorySystem.inventory[i];
                hotbarSlot[i].sprite = item.data.GetIcon();
                hotbarSlot[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                //if empty:
                hotbarSlot[i].sprite = emptySprite;
                Color color = hotbarSlot[i].color;
                color.a = 0f;
                hotbarSlot[i].color = color;

            }
        }
    }
}