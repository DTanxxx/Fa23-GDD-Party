using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Control;
using Lurkers.Environment.Vision;


[System.Serializable] public struct SaveData
{
    public Dictionary<GameObject, TileColor[]> saved;
    public Dictionary<int, bool[]> savedRaised;
    
    public Vector3 checkLoc;
}


public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject[] tileManagers;
    private SaveData dataSaved;
    [SerializeField] private float timer;
    private float TIMER;
    private bool changed = false;
    private bool isDead = false;
    private GameObject[] copies;
    private GameObject newPlayer;
    private Animator animator = null;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        TIMER = timer;
        dataSaved.checkLoc = transform.position;
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
        dataSaved.saved = new Dictionary<GameObject, TileColor[]>();
        dataSaved.savedRaised = new Dictionary<int, bool[]>();
        int k = 0;
        foreach (GameObject tileMan in tileManagers)
        {
            int row = (int)tileMan.GetComponent<ColorTileManager>().matrixSize.x;
            int col = (int)tileMan.GetComponent<ColorTileManager>().matrixSize.y;
            bool[] tileRaised = new bool[row * col];
            TileColor[] colors = new TileColor[row * col];
            
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    colors[(i * col) + j] = tileMan.GetComponent<ColorTileManager>().matrix[i, j].GetComponent<ColorTile>().GetTileColor();
                    tileRaised[(i * col) + j] = tileMan.GetComponent<ColorTileManager>().matrix[i, j].GetComponent<ColorTile>().GetIsRaised();
                }
            }
            dataSaved.saved.Add(tileMan, colors);
            dataSaved.savedRaised.Add(k, tileRaised);
            k++;
        }
        dataSaved.checkLoc = transform.position;
        

    }

    private void whenDead()
    {
        
        bool dead = player.GetComponent<PlayerHealth>().GetIsPlayerDead();
        int manNum = 0;
        if (dead)
        {
            isDead = true;
            foreach (KeyValuePair<GameObject, TileColor[]> manColors in dataSaved.saved)
            {
                GameObject manager = manColors.Key;
                TileColor[] colorArr = manColors.Value;
                bool[] raiseArr = dataSaved.savedRaised[manNum];
                int row = (int)manager.GetComponent<ColorTileManager>().matrixSize.x;
                int col = (int)manager.GetComponent<ColorTileManager>().matrixSize.y;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        if (raiseArr[(i * col) + j] != manager.GetComponent<ColorTileManager>().matrix[i, j].GetComponent<ColorTile>().GetIsRaised())
                        {
                            if (manager.GetComponent<ColorTileManager>().matrix[i, j].GetComponent<ColorTile>().GetIsRaised())
                            {
                                StartCoroutine(manager.GetComponent<ColorTileManager>().matrix[i, j].GetComponent<ColorTile>().Mover(manager.GetComponent<ColorTileManager>().matrix[i, j], "lower"));
                            }
                            else
                            {
                                StartCoroutine(manager.GetComponent<ColorTileManager>().matrix[i, j].GetComponent<ColorTile>().Mover(manager.GetComponent<ColorTileManager>().matrix[i, j], "raise"));
                            }
                        }
                        manager.GetComponent<ColorTileManager>().matrix[i, j].GetComponent<ColorTile>().SetData(colorArr[(i * col) + j], manager.GetComponent<ColorTileManager>(), raiseArr[(i * col) + j]);
                    }
                }
                manNum++;
            }
            deathPanel.SetActive(false);
            if (timer <= 0)
            {
                timer = TIMER;
                isDead = false;
                animator = player.GetComponentInChildren<Animator>();
                player.SetActive(false);
                
                player.transform.position = dataSaved.checkLoc;
                animator.SetTrigger("Respawn");
                player.SetActive(true);
                player.GetComponent<PlayerController>().UnfreezePlayer();
                player.GetComponent<PlayerController>().CheckPointRevive();
                player.GetComponent<PlayerHealth>().SetIsPlayerAlive();
                
            }
        }
        
    }
}
