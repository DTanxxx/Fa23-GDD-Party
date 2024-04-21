using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Hotbar : MonoBehaviour
{
    [SerializeField] Image[] hotbarSlot = new Image[4];
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite Plank; //will be set to placeholder sprite for now
    [SerializeField] Sprite Rope;  //will be set to placeholder sprite for now
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach(Image image in hotbarSlot)
            {
                Color c = image.color;
                if (image.sprite == emptySprite)
                {
                    image.sprite = Plank;
                    c.a = 100;
                    image.color = c;
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach(Image image in hotbarSlot)
            {
                Color c = image.color;
                if (image.sprite == emptySprite)
                {
                    image.sprite = Rope;
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
}
