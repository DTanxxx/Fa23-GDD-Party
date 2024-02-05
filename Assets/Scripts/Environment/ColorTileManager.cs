using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorTileManager : MonoBehaviour
{
    [Tooltip("This should match the size of tile prefab's ColorTile transform scales")]
    [SerializeField] private float tileSize = 6f;
    [SerializeField] private TileSpawner[] tileLocs;
    [SerializeField] private Vector2 matrixSize;

    private List<GameObject> redWhiteList = new List<GameObject>();
    private List<GameObject> greenList = new List<GameObject>();
    private List<(GameObject, bool)> blackList = new List<(GameObject, bool)> ();

    private GameObject[,] matrix;

    private Dictionary<int, GameObject> rowDictionary = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> columnDictionary = new Dictionary<int, GameObject>();
    private Dictionary<GameObject, (int, int)> magentaDictionary = new Dictionary<GameObject, (int, int)>();

    private void Awake()
    {
        if (tileLocs == null) { return; }

        matrix = new GameObject[(int)matrixSize.x, (int)matrixSize.y];

        foreach (TileSpawner tile in tileLocs)
        {
            TileColor color = tile.getColor();
            placeTile(tile, color);
        }
    }
    private void placeTile(TileSpawner tile, TileColor color)
    {
        GameObject obj = tile.getPrefab();
        obj.GetComponent<ColorTile>().SetColor(color);

        foreach (Vector2Int vec in tile.getRowCol())
        {
            int row = vec[0];
            int col = vec[1];

            Vector3 loc;

            if (tile.raised)
            {
                loc = new Vector3(col * tileSize, 2.5f, row * tileSize);
            }

            else
            {
                loc = new Vector3(col * tileSize, 0.11f, row * tileSize);
            }

            //loc += transform.position;

            //GameObject placed = Instantiate(obj, loc, Quaternion.identity);
            GameObject placed = Instantiate(obj, transform);
            placed.transform.localPosition = loc;

            switch (color)
            {
                case TileColor.White:
                    redWhiteList.Add(placed);
                    break;

                case TileColor.Red:
                    redWhiteList.Add(placed);
                    break;

                case TileColor.Black:
                    blackList.Add((placed, tile.raised));
                    break;

                case TileColor.Green:
                    greenList.Add(placed);
                    break;

                case TileColor.Magenta:
                    magentaDictionary.TryAdd(placed, (row, col));
                    break;
            }

            matrix[row, col] = placed;
        }
    }

    public void ActivateCyan()
    {
        Debug.Log(redWhiteList.Count);
        if (redWhiteList != null)
        {
            foreach (GameObject tile in redWhiteList)
            {
                ColorTile found = tile.GetComponent<ColorTile>();
                if (found.GetTileColor() == TileColor.Red)
                {
                    found.TurnWhite();
                }
                else if (found.GetTileColor() == TileColor.White)
                {
                    found.TurnRed();
                }
            }
        }
    }

    public void ActivateMagenta(GameObject tile)
    {
        (int, int) rowcol = magentaDictionary[tile];
        int row = rowcol.Item1;
        int col = rowcol.Item2;
        
        //search row
        for (int i = 0; i < matrixSize.x; i++)
        {
            GameObject inMatrix = matrix[i, col];
            if (inMatrix == null) { continue; }

            if (inMatrix.TryGetComponent<ColorTile>(out var exist))
            {
                ColorTile found = exist;

                if (found.GetTileColor() == TileColor.White)
                {
                    found.TurnRed();
                }
                else if (found.GetTileColor() == TileColor.Red)
                {
                    found.TurnWhite();
                }
            }
        }

        // search column
        for (int j = 0; j < matrixSize.y; j++)
        {
            GameObject inMatrix = matrix[row, j];

            if (inMatrix.TryGetComponent<ColorTile>(out var exist))
            {
                ColorTile found = exist;

                if (found.GetTileColor() == TileColor.White)
                {
                    found.TurnRed();
                }
                else if (found.GetTileColor() == TileColor.Red)
                {
                    found.TurnWhite();
                }
            }
        }
    }

    public void ActivateGreen()
    {
        Debug.Log(greenList.Count);
        foreach (GameObject tile in greenList)
        {
            ColorTile colorTile = tile.GetComponent<ColorTile>();
            if (colorTile != null)
            {
                StartCoroutine(colorTile.Mover(tile, "lower"));
            }
        }
    }

    public void ActivateBlack()
    {
        List<(GameObject tile, bool raised)> nextBlackList = new List<(GameObject tile, bool raised)>();
        foreach ((GameObject tile, bool raised) in blackList)
        {
            ColorTile colorTile = tile.GetComponent<ColorTile>();
            if (colorTile != null)
            {
                if (raised)
                {
                    StartCoroutine(colorTile.Mover(tile, "lower"));
                }
                else
                {
                    StartCoroutine(colorTile.Mover(tile, "raise"));
                }
                nextBlackList.Add((tile, !raised));
            }
        }
        blackList = nextBlackList;
    }
}
    
