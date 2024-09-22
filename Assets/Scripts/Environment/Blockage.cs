using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class Blockage : MonoBehaviour
{
    public NavMeshSurface navSurface;
    public GameObject blockage;
    private bool block;
    // Start is called before the first frame update
    void Start()
    {
        block = false;   
    }

    // Update is called once per frame
    void OnTriggerExit(Collider other)
    {
        if (!block)
        blockage.SetActive(true);
        navSurface.BuildNavMesh();
    }
}
