using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RefillStation : MonoBehaviour
{
    [SerializeField] private int ingredientID;
    
    private Flavor flav;
    private int[] ranges = new int[5];


    
    
    // Start is called before the first frame update
    void Start()
    {
        flav = ScriptableObject.CreateInstance<Flavor>();
        flav.sweet = UnityEngine.Random.Range(0, 20) * 5;
        ranges[0] = flav.sweet;
        flav.bitter = UnityEngine.Random.Range(0, 20) * 5;
        ranges[1] = flav.bitter;
        flav.sour = UnityEngine.Random.Range(0, 20) * 5;
        ranges[2] = flav.sour;
        flav.salty = UnityEngine.Random.Range(0, 20) * 5;
        ranges[3] = flav.salty;
        flav.umami = UnityEngine.Random.Range(0, 20) * 5;
        ranges[4] = flav.umami;

        if (ranges.Count(c =>c == 0) > 3) 
        {
            if (flav.sweet == 0) 
            {
                flav.sweet = UnityEngine.Random.Range(1, 20) * 5;
            } 
            else if (flav.bitter == 0) 
            {
                flav.bitter = UnityEngine.Random.Range(1, 20) * 5;
            }
            else if (flav.sour == 0) 
            {
                flav.sour = UnityEngine.Random.Range(1, 20) * 5;
            }
            else if (flav.salty == 0) 
            {
                flav.salty = UnityEngine.Random.Range(1, 20) * 5;
            }
            else
            {
                flav.umami = UnityEngine.Random.Range(1, 20) * 5;
            }
        }
        
        Debug.Log("Refill Station " + ingredientID + " has a sweet level of " + flav.sweet);
        Debug.Log("Refill Station " + ingredientID + " has a bitter level of " + flav.bitter);
        Debug.Log("Refill Station " + ingredientID + " has a sour level of " + flav.sour);
        Debug.Log("Refill Station " + ingredientID + " has a salty level of " + flav.salty);
        Debug.Log("Refill Station " + ingredientID + " has a umami level of " + flav.umami);
    }


    public Flavor getFlavor() 
    {
        return flav;
    }

    

    [CreateAssetMenu]
    public class Flavor : ScriptableObject
    {
        [Range(0, 100)]
        public int sweet;

        [Range(0, 100)]
        public int bitter;

        [Range(0, 100)]
        public int sour;

        [Range(0, 100)]
        public int salty;

        [Range(0, 100)]
        public int umami;
    }


    public class Formula : MonoBehaviour
    {
        public static Flavor Combine(Flavor A, Flavor B)
        {
            Flavor newFlavor = ScriptableObject.CreateInstance<Flavor>();
            newFlavor.sweet = (A.sweet + B.sweet) / 2;
            newFlavor.bitter = (A.bitter + B.bitter) / 2;
            newFlavor.salty = (A.salty + B.salty) / 2;
            newFlavor.sour = (A.sour + B.sour) / 2;
            newFlavor.umami = (A.umami + B.umami) / 2;
            return newFlavor;
            
        }
    }
}
