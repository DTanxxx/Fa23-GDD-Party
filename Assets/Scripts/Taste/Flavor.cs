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

    public override bool Equals(object someFlav)
    {
        if (this == null || someFlav == null || someFlav is not Flavor)
        {
            return false;
        }
        
        Flavor inpFlav = (Flavor) someFlav;
        float tolerance = 0.1f;  
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

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
