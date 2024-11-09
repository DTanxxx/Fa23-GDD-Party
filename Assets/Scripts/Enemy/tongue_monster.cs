using FunkyCode.SuperTilemapEditorSupport.Light.Shadow;
using JetBrains.Annotations;
using Lurkers.Control;
using Lurkers.Vision;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class tongue_monster : MonoBehaviour
{
    [SerializeField] private float timeattacks = 2.0f;
    [SerializeField] private UnityEngine.Collider myCollider = null;
    private NavMeshAgent agent;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float attackTimer = 2f;
    [SerializeField] private float DefaultattackTimer = 2f;
    [SerializeField] Flavor VulnerableFlavorComp; 


    private GameObject player;
    private PlayerController playerControl;
    private bool isPlayerDead = false;
    private float freezeTimer = 0f;
    private int num_of_encounter = 0;
    private bool isDead = false;
    private bool cooldown = false;
    private int attackType = 1;
    public Flavor[] flavors = new Flavor[2];




    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.FindObjectOfType<PlayerController>();
        Debug.Log(player);
        agent = GetComponent<NavMeshAgent>();
        //playerControl = player.GetComponent<PlayerController>();
        Debug.Log(playerControl);

        // playerControl.flavorCombinations = new List<Flavor>();
        //test flavor 0.1 salty + 0.1 bitter 
        Flavor newFlavor = ScriptableObject.CreateInstance<Flavor>();
        newFlavor.salty = 0.1f;
        newFlavor.umami = 0.0f;
        newFlavor.bitter = 0.1f;
        newFlavor.sour = 0.0f;
        newFlavor.sweet = 0.0f;
        flavors[0] = newFlavor;

        Debug.Log(newFlavor);
        if (playerControl == null)
        {
            Debug.LogError("PlayerControl is not assigned!");
        }
        // playerControl.SetFlavor(flavors);

        

    }

    private void OnEnable()
    {
        PlayerHealth.onDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        PlayerHealth.onDeath -= OnPlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        //react to flavor test
        ReactToFlavor();

        if (isPlayerDead || isDead)
        {
            return;
        }


        // game object is enabled 

        //Input.GetKeyDown(KeyCode.M) &&
        if (Input.GetKeyDown(KeyCode.L) && num_of_encounter < 4 && compareFlavorAttributes(flavors[0], VulnerableFlavorComp)) // to do when chemical concoction is thrown at it
        {
            // disables movement for 20 seconds 
            if (num_of_encounter == 0)
            {
                
                freezeTimer = 20f;
                StartCoroutine(Freeze());
                num_of_encounter++;
            }

            // disables movement for 15 seconds

            if (num_of_encounter == 1)
            {
                freezeTimer = 15f;
                StartCoroutine(Freeze());
                num_of_encounter++;
            }

            // disables movement for 10 seconds

            if (num_of_encounter == 2)
            {
                freezeTimer = 10f;
                StartCoroutine(Freeze());
                num_of_encounter++;
            }

            // disables movement for 5 seconds

            if (num_of_encounter == 3)
            {
                freezeTimer = 5f;
                StartCoroutine(Freeze());
                num_of_encounter++;
            }
        }
        // disables movement for 0 seconds / permantely enabled
        // (done by default) with new update call

        // some distance from player locator
        if (!cooldown) //other condtion: Vector3.Distance(player.transform.position, transform.position) 
        { 
            //different attack types 
            if (attackType == 1)
            {
                attackPattern1();
                AttackTypeTimer();
            }

            if (attackType == 2)
            {
                attackPattern2();
                AttackTypeTimer();
            }

            if (attackType == 3)
            {
                attackPattern3();
                AttackTypeTimer();
            }
         
        }

    }

    IEnumerator Freeze()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(freezeTimer);
        gameObject.SetActive(true);
    }

    bool compareFlavorAttributes(Flavor flav1, Flavor flav2)
    {
        if (flav1.bitter != flav2.bitter 
            && flav1.umami != flav2.umami
            && flav1.sweet != flav2.sweet
            && flav1.salty != flav2.salty
            && flav1.sour != flav2.sour) {
            return false; 
        }
        return true; 
    }

    void AttackTypeTimer()
    {
        cooldown = true; 
        attackTimer -= Time.deltaTime; 
        if (attackTimer < 0)
        {
            cooldown = false;
            attackType = Random.Range(1, 4);
            attackTimer = DefaultattackTimer;
        }
    }


    void attackPattern1 ()
    {
        Debug.Log("attack 1");
    }

    void attackPattern2()
    {
        Debug.Log("attack 2");
    }

    void attackPattern3()
    {
        Debug.Log("attack 3");
    }

    private void OnPlayerDeath(DeathCause cause, Vector3 enemyPosition, GameObject enemy = null)
    {
        if (cause == DeathCause.FACEHUGGER && enemy == gameObject) // change to tongue monster
        {
            spriteRenderer.enabled = false;
        }

        myCollider.enabled = false;
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        isPlayerDead = true;

        gameObject.SetActive(false);
    }

    public void ReactToFlavor()
    {
        if (playerControl != null && playerControl.flavorCombinations != null)
        {
            if (playerControl.flavorCombinations.Contains(flavors[0]) && Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("Combination Flavor - Sweet: " + flavors[0].sweet);
                Debug.Log("Combination Flavor - Bitter: " + flavors[0].sour);
                Debug.Log("Combination Flavor - Bitter: " + flavors[0].bitter);
                Debug.Log("Combination Flavor - Bitter: " + flavors[0].salty);
                Debug.Log("Combination Flavor - Bitter: " + flavors[0].umami);
            }
        }
    }

}
