using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lurkers.Inventory
{
    [CreateAssetMenu(menuName = "Inventory Item Data")]
    public class ItemData : ScriptableObject
    {
        public string id;
        public string displayName;
        public bool stackable = true;
        public Sprite icon;
        public GameObject prefab;
        public AudioClip itemSFX;

        public virtual Sprite GetIcon()
        {
            return icon;
        }

        //returns true if there is an interaction
        public virtual bool Interact(ItemData someItem)
        {
            return false;
        }
    }
}