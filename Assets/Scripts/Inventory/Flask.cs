using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Objective;
using UnityEditor;
using Unity.VisualScripting;


namespace Lurkers.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Flask Item", fileName = "New Flask Item")]
    public class Flask : ItemData
    {
        //reference flavor
        [SerializeField] Sprite emptyFlask;
        [SerializeField] Flavor refFlav;
        public bool selected = false;
        public bool full = true;

        public override void ClickAction()
        {
            base.ClickAction();
            if (selected)
            {
                selected = false;
            }
            else if (prefab.Equals(refFlav))
            {
                selected = true;
            }
        }

        public void mergeFlask(Flask someFlask)
        {
            if (someFlask.full && full)
            {
                refFlav = Formula.Combine(refFlav, someFlask.getFlavor());
                full = false;
                icon = emptyFlask;
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

