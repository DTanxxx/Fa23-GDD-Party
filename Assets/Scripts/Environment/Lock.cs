using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private Flavor exactFlav; 
    private bool isLocked = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //attempts to openLock
    public void Dissolve(Flavor flavor)
    {
        if (isLocked && flavor.Equals(exactFlav))
        {
            isLocked = false;
        }
    }

    public bool getLocked { get {  return isLocked; } }
}
