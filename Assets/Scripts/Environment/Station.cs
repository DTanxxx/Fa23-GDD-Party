using UnityEngine;
using System;

public class Station: MonoBehaviour
{
    public static Flavor MultiplyComponents(Flavor originalFlavor, float multiplier)
    {
        Flavor newFlavor = ScriptableObject.CreateInstance<Flavor>();

        newFlavor.sweet = (int) Math.Clamp(originalFlavor.sweet * multiplier, 0, 100);
        newFlavor.bitter = (int) Math.Clamp(originalFlavor.bitter * multiplier, 0, 100);
        newFlavor.sour = (int) Math.Clamp(originalFlavor.sour * multiplier, 0, 100);
        newFlavor.salty = (int) Math.Clamp(originalFlavor.salty * multiplier, 0, 100);
        newFlavor.umami = (int) Math.Clamp(originalFlavor.umami * multiplier, 0, 100);

        return newFlavor;
    }
}
