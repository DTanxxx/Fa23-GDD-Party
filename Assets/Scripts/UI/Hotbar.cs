using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lurkers.Inventory;


public class Hotbar : MonoBehaviour
{
    [SerializeField] private Image[] hotbarSlot = new Image[4];
    private Queue<ItemData> itemsQueue = new Queue<ItemData>();
    [SerializeField] private GameObject player;
    [SerializeField] private Sprite emptySprite;
    private bool inHotBar = false;
    //[SerializeField] private Sprite Plank; //will be set to placeholder sprite for now
    //[SerializeField] private Sprite Rope;  //will be set to placeholder sprite for now
    

    // Update is called once per frame
    void Update()
    {
        foreach (Image slot in hotbarSlot)
        {
            Color c = slot.color;
            if (slot.sprite == emptySprite && itemsQueue.Count != 0)
            {
                slot.sprite = itemsQueue.Dequeue().icon;
                c.a = 100;
                slot.color = c;
            }
        }
    }

    public void showItem(ItemData itemToAdd)
    {
        itemsQueue.Enqueue(itemToAdd);
    }

    public void removeItem(ItemData itemToDelete)
    {
        for (int i = 0; i < hotbarSlot.Length; i++)
        {
            Color c = hotbarSlot[i].color;
            if (hotbarSlot[i].sprite == itemToDelete.icon)
            {
                hotbarSlot[i].sprite = emptySprite;
                c.a = 0;
                hotbarSlot[i].color = c;
                inHotBar = true;
                break; 
            }
        }

        if (!inHotBar)
        {
            Queue<ItemData> temp = new Queue<ItemData>();
            int size = itemsQueue.Count;
            int cnt = 0;
            while (itemsQueue.Peek().id != itemToDelete.id  && itemsQueue.Count != 0)
            {
                temp.Enqueue(itemsQueue.Peek());
                itemsQueue.Dequeue();
                cnt++;
            }
            itemsQueue.Dequeue();
            while (temp.Count != 0)
            {
                itemsQueue.Enqueue(temp.Peek());
                temp.Dequeue();
            }
            int k = size - cnt - 1;
            while (k > 0)
            {
                ItemData curr = itemsQueue.Peek();
                itemsQueue.Dequeue();
                itemsQueue.Enqueue(curr);
            }

        }
    }

}
