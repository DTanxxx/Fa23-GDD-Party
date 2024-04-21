using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] tileManagers;
    private GameObject[] copies;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        whenDead();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            copies = new GameObject[tileManagers.Length];
            for (int i = 0; i < tileManagers.Length; i++)
            {
                copies[i] = Instantiate(tileManagers[i]);
            }
        }
    }

    private void whenDead()
    {
        bool dead = true;
        if (dead)
        {
            for (int i = 0; i < copies.Length; i++)
            {
                tileManagers[i] = copies[i];
            }
            player.transform.position = transform.position;
        }
    }
}
