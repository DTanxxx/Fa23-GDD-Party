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
        public Sprite icon;
        public GameObject prefab;
    }
}