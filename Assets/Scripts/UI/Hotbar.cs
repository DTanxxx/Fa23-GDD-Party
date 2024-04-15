using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Hotbar : MonoBehaviour
{
    [SerializeField] Image[] hotbarSlot = new Image[5];
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
                if (image.sprite == emptySprite)
                {
                    image.sprite = Plank;
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach(Image image in hotbarSlot)
            {
                if (image.sprite == emptySprite)
                {
                    image.sprite = Rope;
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            for (int i = 0; i < hotbarSlot.Length; i++)
            {
                if (hotbarSlot[i].sprite == Plank)
                {
                    hotbarSlot[i].sprite = emptySprite;
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            for (int i = 0; i < hotbarSlot.Length; i++)
            {
                if (hotbarSlot[i].sprite == Rope)
                {
                    hotbarSlot[i].sprite = emptySprite;
                    break;
                }
            }
        }
    }
}
