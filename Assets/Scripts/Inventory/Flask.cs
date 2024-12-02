using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Objective;
using UnityEditor;
using Unity.VisualScripting;
using Lurkers.Audio;


namespace Lurkers.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Flask Item", fileName = "New Flask Item")]
    public class Flask : ItemData
    {
        //reference flavor
        //do not set emptyFlask the same sprite as emptySprite
        [SerializeField] Sprite emptyFlask;
        [SerializeField] Flavor refFlav;
        public bool full = true;

        public override Sprite GetIcon()
        {
            if (full)
            {
                return icon;
            }
            return emptyFlask;
        }

        public override bool Interact(ItemData someItem)
        {
            if (someItem is Flask targetFlask &&
                full && targetFlask.full)
            {
                Debug.Log("flask 1before " +  refFlav);
                mergeFlask(targetFlask);
                Debug.Log("flask 1after " +  targetFlask.refFlav);
                return true;
            }
            return false;
        }

        public void mergeFlask(Flask someFlask)
        {
            if (someFlask.full && full)
            {
                someFlask.refFlav = Formula.Combine(refFlav, someFlask.getFlavor());
                full = false;
            }
        }

        public void setFlav(Flavor someFlav)
        {
            refFlav = someFlav; 
        }

        public Flavor getFlavor()
        {
            return refFlav;
        }

        public Sprite GetEmpty()
        {
            return emptyFlask;
        }
    }
}

