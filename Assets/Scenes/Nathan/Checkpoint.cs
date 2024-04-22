using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Control;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject[] tileManagers;
    [SerializeField] private float timer;
    private float TIMER;
    private bool changed = false;
    private bool isDead = false;
    private GameObject[] copies;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        TIMER = timer;
    }

    // Update is called once per frame
    void Update()
    {
        whenDead();
        if (isDead && timer > 0)
        {
            timer -= Time.deltaTime;
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !changed)
        {
            changed = true;
            copies = new GameObject[tileManagers.Length];
            for (int i = 0; i < tileManagers.Length; i++)
            {
                copies[i] = tileManagers[i];
            }
        }
    }

    private void whenDead()
    {
        
        bool dead = player.GetComponent<PlayerHealth>().GetIsPlayerDead();
        if (dead)
        {
            isDead = true;
            for (int i = 0; i < copies.Length; i++)
            {
                tileManagers[i] = copies[i];
            }
            deathPanel.SetActive(false);
            if (timer <= 0)
            {
            player.GetComponent<PlayerController>().UnfreezePlayer();
            player.GetComponent<PlayerController>().isDead = false;
            player.GetComponent<PlayerHealth>().isDead = false;
            player.transform.position = transform.position;
            timer = TIMER;
            }
        }
        
    }
}
