using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Flavor : ScriptableObject, ITastable //ScriptableObject
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

    private bool inside;

    private void OnTriggerEnter(Collider collision)
    {
        GameObject player = collision.transform.parent.parent.gameObject;;

        if (!player.gameObject.CompareTag("Player"))
        {
            return;
        }
        inside = true;
    }
    private void OnTriggerExit(Collider collision)
    {
        GameObject player = collision.transform.parent.parent.gameObject;

        if (!player.gameObject.CompareTag("Player"))
        {
            return;
        }
        inside = false;
    }
    public void printComp(float sweet, float bitter, float sour, float salty, float umami)
    {
        Debug.Log($"Sweet: {sweet}, Bitter: {bitter}, Sour: {sour}, Salty: {salty}, Umami: {umami}");
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.Q) && inside)
        {
            printComp(sweet, bitter, sour, salty, umami);
        }
    }

}
