using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyActivate : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private ItemObject activationItem;
    [SerializeField] private WeepingAngelMovement[] weepingAngels;

    private ItemData itemData;
    private bool active = false;

    private void Awake()
    {
        if (activationItem != null)
        {
            itemData = activationItem.GetItemData();
        }
    }
    
    private void Update()
    {
        if (active || activationItem == null)
        {
            return;
        }

        if (inventorySystem.Get(itemData) != null)
        {
            if (inventorySystem.Get(itemData).stackSize == 1)
            {
                //Debug.Log("awake the beasts");
                SetActive();
            }
        }
    }

    public void SetActive()
    {
        foreach (var enemy in weepingAngels)
        {
            if (enemy != null)
            {
                enemy.SetActive();
                active = true;
            }
        }
    }
}
