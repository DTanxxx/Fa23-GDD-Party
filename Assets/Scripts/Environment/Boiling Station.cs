using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilingStation : Station
{
    [SerializeField] static float multiplier = 2f;

    public static Flavor MultiplyComponents(Flavor originalFlavor)
    {
        return Station.MultiplyComponents(originalFlavor, multiplier);
    }
}
