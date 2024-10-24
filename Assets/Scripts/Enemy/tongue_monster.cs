using FunkyCode.SuperTilemapEditorSupport.Light.Shadow;
using JetBrains.Annotations;
using Lurkers.Control;
using Lurkers.Vision;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] List<Flavor> VulnerableFlavorComp; // 0.1 sweet + 0.2 sour


    private GameObject player;  
    private bool isPlayerDead = false;
    private float freezeTimer = 0f;
    private int num_of_encounter = 0;
    private bool isDead = false;
    private bool cooldown = false;
    private int attackType = 1; 




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
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
        if (isPlayerDead || isDead)
        {
            return;
        }

         // game object is enabled 

        if (Input.GetKeyDown(KeyCode.M) && num_of_encounter < 4) // to do when chemical concoction is thrown at it
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

    /*public void ReactToFlavor()
    {
        if (PlayerController.flavorCombinations != null) // checks if null player flavors
        {
            Flavor playerFlavor = player.currentFlavor;

            // Example of reacting to sweetness
            if (PlayerController.flavorCombinations == VulnerableFlavorComp)
            {
                IncreaseAttraction("sweet");
            }
            

        }
    }*/

}
