using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (flavor.Equals(exactFlav))
        {
            isLocked = false;
            //Debug.Log("Dissolved");
        } 
    }

    public bool getLocked { get {  return isLocked; } }
    public void setFlav(Flavor someFlav) { exactFlav = someFlav; }
   
}
