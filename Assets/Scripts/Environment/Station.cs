using UnityEngine;

public class Station: MonoBehaviour
{
    public static Flavor MultiplyComponents(Flavor originalFlavor, float multiplier)
    {
        Flavor newFlavor = ScriptableObject.CreateInstance<Flavor>();

        newFlavor.sweet = Mathf.Clamp(originalFlavor.sweet * multiplier, 0f, 1f);
        newFlavor.bitter = Mathf.Clamp(originalFlavor.bitter * multiplier, 0f, 1f);
        newFlavor.sour = Mathf.Clamp(originalFlavor.sour * multiplier, 0f, 1f);
        newFlavor.salty = Mathf.Clamp(originalFlavor.salty * multiplier, 0f, 1f);
        newFlavor.umami = Mathf.Clamp(originalFlavor.umami * multiplier, 0f, 1f);

        return newFlavor;
    }
}
