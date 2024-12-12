using Lurkers.Taste;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Environment.Taste
{
    public class Lock : MonoBehaviour
    {
        [SerializeField] private Flavor exactFlav;

        private bool isLocked = true;

        //attempts to openLock
        public void Dissolve(Flavor flavor)
        {
            if (flavor.Equals(exactFlav))
            {
                isLocked = false;
            }
        }

        public bool GetLocked()
        { 
            return isLocked; 
        }

        public void SetFlav(Flavor someFlav) 
        { 
            exactFlav = someFlav; 
        }
    }
}
