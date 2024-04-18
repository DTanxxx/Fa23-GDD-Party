using Lurkers.Control;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerSprinter : MonoBehaviour
{

    public float moveSpeed;
    [SerializeField] private float maxStamina = 5f;
    [SerializeField] private float currStamina;
    [SerializeField] public bool hasRegenerated;
    [SerializeField] public bool weAreSprinting = false; 

    public static PlayerSprinter instance;
    [SerializeField] private float staminaRegen = 1f;
    [SerializeField] private float staminaDrain = 1f;
    [SerializeField] private float slowedRunSpeed = 0.1f;
    [SerializeField] private float normalRunSpeed = 0.2f;
    [SerializeField] private float sprintingRunSpeed = 0.3f;

    // Start is called before the first frame update
    

  
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currStamina = maxStamina;
        hasRegenerated = true; 
        moveSpeed = normalRunSpeed; 
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            weAreSprinting = false;
        }

        if (!weAreSprinting)
        {
            if (currStamina <= maxStamina)
            {
                currStamina += staminaRegen * Time.deltaTime;
                Debug.Log(currStamina);
              
                if (currStamina >= maxStamina)
                {
                    moveSpeed = normalRunSpeed;
                    hasRegenerated = true;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Sprinting();
            }
        }
        GetComponent<PlayerController>().SetRunSpeed(moveSpeed);
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
