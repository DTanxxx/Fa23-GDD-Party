using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula : MonoBehaviour
{
    
    public static Flavor Combine(Flavor A, Flavor B)
    {
        Flavor newFlavor = ScriptableObject.CreateInstance<Flavor>();

        newFlavor.sweet = (A.sweet + B.sweet) / 2;
        newFlavor.bitter = (A.bitter + B.bitter) / 2;
        newFlavor.salty = (A.salty + B.salty) / 2;
        newFlavor.sour = (A.sour + B.sour) / 2;
        newFlavor.umami = (A.umami + B.umami) / 2;

        return newFlavor;

        
    }
    
}
