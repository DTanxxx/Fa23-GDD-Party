using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillInit : MonoBehaviour
{
    [SerializeField] private GameObject[] refillStations;
    [SerializeField] private int[] starts = new int[5];
    [SerializeField] private int[] ends = new int[5];
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject station in refillStations)
        {
            station.GetComponent<RefillStation>().instantiateFlavor();
            for (int i = 0; i < 5; i++)
            {
                station.GetComponent<RefillStation>().setFlavor(i, starts[i], ends[i]);
            }
            station.GetComponent<RefillStation>().moreThan3();
            station.GetComponent<RefillStation>().flavComps();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
