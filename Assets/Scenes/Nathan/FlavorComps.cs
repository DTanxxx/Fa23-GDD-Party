using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavorComps : MonoBehaviour
{
    public Flavor[] refills = new Flavor[5];


    private Flavor NewFlavorComp(Flavor[] flavs, int mixTimes) 
    {
        int cnt = mixTimes;
        int index1 = UnityEngine.Random.Range(0, flavs.Length - 1);
        int index2 = UnityEngine.Random.Range(0, flavs.Length - 1);
        cnt -= 2;
        Flavor newFlavor = Formula.Combine(flavs[index1], flavs[index2]);
        while (cnt < mixTimes) 
        {
            int newIndex = UnityEngine.Random.Range(0, flavs.Length - 1);
            newFlavor = Formula.Combine(newFlavor, flavs[newIndex]);
            cnt -= 1;
        }
        return newFlavor;
    }
}
