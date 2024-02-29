using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] Sprite[] hotbarSlot = new Sprite[5];
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite placeholder1;
    [SerializeField] Sprite placeholder2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (hotbarSlot[0] == emptySprite)
            {
                hotbarSlot[0] = placeholder1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (hotbarSlot[1] == emptySprite)
            {
                hotbarSlot[1] = placeholder2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hotbarSlot[0] = placeholder1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            hotbarSlot[1] = placeholder2;
        }
    }
}
