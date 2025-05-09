﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Taste
{
    public enum FlavorComposite
    {
        Sweet,
        Bitter,
        Sour,
        Salty,
        Umami
    }

    [CreateAssetMenu]
    public class Flavor : ScriptableObject
    {
        [Range(0, 100)]
        public int sweet;

        [Range(0, 100)]
        public int bitter;

        [Range(0, 100)]
        public int sour;

        [Range(0, 100)]
        public int salty;

        [Range(0, 100)]
        public int umami;

        public override bool Equals(object someFlav)
        {
            if (this == null || someFlav == null || someFlav is not Flavor)
            {
                return false;
            }

            Flavor inpFlav = (Flavor)someFlav;
            float tolerance = 2f;
            if (Mathf.Abs(this.sweet - inpFlav.sweet) < tolerance &&
                Mathf.Abs(this.bitter - inpFlav.bitter) < tolerance &&
                Mathf.Abs(this.sour - inpFlav.sour) < tolerance &&
                Mathf.Abs(this.salty - inpFlav.salty) < tolerance &&
                Mathf.Abs(this.umami - inpFlav.umami) < tolerance)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return ("sweet: " + sweet + "bitter: " + bitter + "sour: " + sour + "salty: " + salty + "umami: " + umami);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}