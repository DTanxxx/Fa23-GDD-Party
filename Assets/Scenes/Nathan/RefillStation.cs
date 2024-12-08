using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;

public class RefillStation : MonoBehaviour
{
    [SerializeField] private int ingredientID;
    
    private Flavor flav;
    [SerializeField] private GameObject FlavorComps;
    private int[] ranges = new int[5];




    // void Update()
    // {
    //     Debug.Log("Refill Station " + ingredientID + " has a sweet level of " + flav.sweet);
    //     Debug.Log("Refill Station " + ingredientID + " has a bitter level of " + flav.bitter);
    //     Debug.Log("Refill Station " + ingredientID + " has a sour level of " + flav.sour);
    //     Debug.Log("Refill Station " + ingredientID + " has a salty level of " + flav.salty);
    //     Debug.Log("Refill Station " + ingredientID + " has a umami level of " + flav.umami);
    // }
    public void instantiateFlavor()
    {
        flav = ScriptableObject.CreateInstance<Flavor>();
    }
    public Flavor getFlavor() 
    {
        return flav;
    }
    public void flavComps()
    {
        FlavorComps.GetComponent<FlavorComps>().refills[ingredientID] = flav;
        // Debug.Log("Refill Station " + ingredientID + " has a sweet level of " + flav.sweet);
        // Debug.Log("Refill Station " + ingredientID + " has a bitter level of " + flav.bitter);
        // Debug.Log("Refill Station " + ingredientID + " has a sour level of " + flav.sour);
        // Debug.Log("Refill Station " + ingredientID + " has a salty level of " + flav.salty);
        // Debug.Log("Refill Station " + ingredientID + " has a umami level of " + flav.umami);
    }
    public void setFlavor(int flavType, int start, int end) 
    {
        //0 - sweet, 1 - bitter, 2 - sour, 3 - salty, 4 - umami
        switch (flavType) 
        {
            case 0:
                flav.sweet = UnityEngine.Random.Range(start, end) * 5;
                ranges[0] = flav.sweet;
                break;
            case 1:
                flav.bitter = UnityEngine.Random.Range(start, end) * 5;
                ranges[1] = flav.bitter;
                break;
            case 2:
                flav.sour = UnityEngine.Random.Range(start, end) * 5;
                ranges[2] = flav.sour;
                break;
            case 3:
                flav.salty = UnityEngine.Random.Range(start, end) * 5;
                ranges[3] = flav.salty;
                break;
            case 4:
                flav.umami = UnityEngine.Random.Range(start, end) * 5;
                ranges[4] = flav.umami;
                break;
        }
    }

    public void moreThan3() 
    {
        if (ranges.Count(c => c == 0) > 3) 
        {
            if (flav.sweet == 0) 
            {
                flav.sweet = UnityEngine.Random.Range(1, 20) * 5;
                ranges[0] = flav.sweet;
            } 
            else if (flav.bitter == 0) 
            {
                flav.bitter = UnityEngine.Random.Range(1, 20) * 5;
                ranges[1] = flav.bitter;
            }
            else if (flav.sour == 0) 
            {
                flav.sour = UnityEngine.Random.Range(1, 20) * 5;
                ranges[2] = flav.sour;
            }
            else if (flav.salty == 0) 
            {
                flav.salty = UnityEngine.Random.Range(1, 20) * 5;
                ranges[3] = flav.salty;
            }
            else
            {
                flav.umami = UnityEngine.Random.Range(1, 20) * 5;
                ranges[4] = flav.umami;
            }
        }
    }

    

    


    
}
