using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteAnimation : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Playing()
    {
        animator.SetBool("ButtonPressed", true);
        //Invoke("StopPlay", wait_time);
    }
    private void StopPlay()
    {
        animator.SetBool("ButtonPressed", false);
    }
}
