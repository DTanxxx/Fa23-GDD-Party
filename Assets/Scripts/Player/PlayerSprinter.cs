using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerSprinter : MonoBehaviour
{

    public float moveSpeed; 
   
    private float maxStamina = 100.0f;
    private float currStamina;
    public bool hasRegenerated = true;
    public bool weAreSprinting = false; 

    public static PlayerSprinter instance;
    private float staminaRegen = 0.5f;
    private float staminaDrain = 0.5f;
    private float slowedRunSpeed = 0.1f;
    private float normalRunSpeed = 0.2f;
    private float sprintingRunSpeed = 0.3f;

    // Start is called before the first frame update
    

  
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currStamina = maxStamina; 
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!weAreSprinting)
        {
            if (currStamina <= maxStamina - 0.01)
            {
                currStamina += staminaRegen * Time.deltaTime;

                if (currStamina >= maxStamina)
                {
                    moveSpeed = normalRunSpeed;
                    hasRegenerated = true;
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Sprinting();
                Debug.Log("hi");
            }
        }
    }

    public void Sprinting()
    {
        if (hasRegenerated)
        {
            weAreSprinting = true;
            currStamina -= staminaDrain * Time.deltaTime;
            moveSpeed = sprintingRunSpeed;

            if (currStamina <= 0)
            {
                hasRegenerated = false;
                moveSpeed = slowedRunSpeed;
            }
        }
    }


}
