using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spillage : MonoBehaviour, ITastable
{

    [SerializeField] public Flavor spill;
    private bool inside;

    private void OnTriggerEnter(Collider collision)
    {
        GameObject player = collision.transform.parent.parent.gameObject; ;

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
    public void printComp()
    {
        Debug.Log($"Sweet: {spill.sweet}, Bitter: {spill.bitter}, Sour: {spill.sour}, Salty: {spill.salty}, Umami: {spill.umami}");
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) && inside)
        {
            printComp();
        }
    }
}
