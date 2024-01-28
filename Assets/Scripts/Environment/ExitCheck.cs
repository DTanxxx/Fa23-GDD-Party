using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCheck : MonoBehaviour
{
    private Collider _collider;
    private GameObject _parent;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        _parent = transform.parent.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_parent != null)
        {
            ColorTile tile = _parent.GetComponent<ColorTile>();
            tile.EnterTile();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GameObject player = other.transform.parent.parent.gameObject;

        Debug.Log(other.gameObject.name);

        if (!player.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (_parent != null)
        {
            ColorTile tile = _parent.GetComponent<ColorTile>();
            tile.ExitTile();
        }
    }
}
