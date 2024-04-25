using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Control;
using UnityEditor.Animations;
using Lurkers.Environment.Vision.ColorTile;


[System.Serializable] public struct SaveData
{
    public Dictionary<GameObject, TileColor[]> saved;
    public bool[] tileRaised;
    public Vector3 checkLoc;
}


public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject[] tileManagers;
    private SaveData data;
    //private Dictionary<GameObject, TileColor[]> save;
    //private bool[] tileRaised;
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
            saveFunctionality();
        }
    }

    private void saveFunctionality()
    {
        foreach (GameObject tileMan in tileManagers)
        {
            TileColor[] colors = new TileColor[(int)tileMan.GetComponent<ColorTileManager>().matrixSize.x * (int)tileMan.GetComponent<ColorTileManager>().matrixSize.y];
            data.tileRaised = new bool[(int)tileMan.GetComponent<ColorTileManager>().matrixSize.x * (int)tileMan.GetComponent<ColorTileManager>().matrixSize.y];
            int row = (int)tileMan.GetComponent<ColorTileManager>().matrixSize.x;
            int col = (int)tileMan.GetComponent<ColorTileManager>().matrixSize.y;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    colors[(i * row) + j] = tileMan.GetComponent<ColorTileManager>().matrix[i, j].GetComponent<ColorTile>().GetTileColor();
                    data.tileRaised[(i * row) + j] = tileMan.GetComponent<ColorTileManager>().matrix[i, j].GetComponent<ColorTile>().GetIsRaised();
                }
            }
            data.saved.Add(tileMan, colors);
        }
        data.checkLoc = transform.position;


    }

    private void whenDead()
    {
        
        bool dead = player.GetComponent<PlayerHealth>().GetIsPlayerDead();
        if (dead)
        {
            isDead = true;
            Dictionary<GameObject, TileColor[]>.KeyCollection keyColl = data.saved.Keys;
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
