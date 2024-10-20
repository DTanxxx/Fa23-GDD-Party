using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Flavor : ScriptableObject
{
    [Range(0f, 1f)]
    public float sweet;

    [Range(0f, 1f)]
    public float bitter;

    [Range(0f, 1f)]
    public float sour;

    [Range(0f, 1f)]
    public float salty;

    [Range(0f, 1f)]
    public float umami;

    bool Equals(Object someFlav)
    {
        if (this == null || someFlav == null || someFlav is not Flavor)
        {
            return false;
        }
        
        Flavor inpFlav = (Flavor) someFlav;
        if (this.sweet == inpFlav.sweet &&
            this.bitter == inpFlav.bitter &&
            this.sour == inpFlav.sour &&
            this.salty == inpFlav.salty &&
            this.umami == inpFlav.umami)
        {
            return true;
        }
        return false;
    }


}
