using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkPointOnOff : MonoBehaviour
{
    [SerializeField] GameObject[] checkpoints;
    private bool activated = false;
    


    public void onButtonPressed()
    {
        activated = true;
        foreach (GameObject checkpoint in checkpoints)
        {
            if (checkpoint.activeInHierarchy)
            {
                checkpoint.SetActive(false);

            } 
            else 
            {
                checkpoint.SetActive(true);
            }
        }
    }

    public bool isActivated()
    {
        return activated;
    }

}
