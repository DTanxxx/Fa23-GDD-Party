using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class EnemyActivate : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private ItemObject activationItem;

    private WeepingAngelMovement[] weepingAngels;
    private ItemData itemData;
    private bool active = false;

    private void Awake()
    {
        weepingAngels = FindObjectsOfType<WeepingAngelMovement>();
        Debug.Log(weepingAngels.Length);
        itemData = activationItem.GetItemData();
    }
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            return;
        }

        if (inventorySystem.Get(itemData) != null)
        {
            if (inventorySystem.Get(itemData).stackSize == 1)
            {
                Debug.Log("awake the beasts");
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
