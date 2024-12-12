using Lurkers.Taste;
using UnityEngine;

namespace Lurkers.Inventory
{
    [CreateAssetMenu(menuName = "Flask")]
    public class Flask : ItemData
    {
        [SerializeField] Sprite emptyFlaskSprite;
        [SerializeField] Flavor refFlav;

        public bool full = true;

        public override Sprite GetIcon()
        {
            if (full)
            {
                return icon;
            }
            return emptyFlaskSprite;
        }

        public override bool Interact(ItemData someItem)
        {
            if (someItem is Flask targetFlask &&
                full && targetFlask.full)
            {
                MergeFlask(targetFlask);
                return true;
            }
            return false;
        }

        public void MergeFlask(Flask someFlask)
        {
            if (someFlask.full && full)
            {
                someFlask.refFlav = Formula.Combine(refFlav, someFlask.GetFlavor());
                full = false;
            }
        }

        public void SetFlav(Flavor someFlav)
        {
            refFlav = someFlav;
            full = true;
        }

        public Flavor GetFlavor()
        {
            if (!full)
            {
                return null;
            }
            return refFlav;
        }

        public Color GetColor()
        {
            Color c = Color.white;

            if (full)
            {
                c.r = (refFlav.sweet + refFlav.bitter + refFlav.sour) / 300f;
                c.g = (refFlav.bitter + refFlav.sour + refFlav.salty) / 300f;
                c.b = (refFlav.sour + refFlav.salty + refFlav.umami) / 300f;
            }
            
            return c;
        }
    }
}

