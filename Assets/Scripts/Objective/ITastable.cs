using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITastable
{
    /*float sweet { get;  }
    float bitter { get; }
    float sour { get; }
    float salty { get; }
    float umami { get; }
    */
    public void printComp (float sweet, float bitter, float sour, float salty, float umami)
    {
        Debug.Log($"Sweet: {sweet}, Bitter: {bitter}, Sour: {sour}, Salty: {salty}, Umami: {umami}");
    }
}
