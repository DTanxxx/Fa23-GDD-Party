using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Flavor : ScriptableObject //ScriptableObject
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
}
