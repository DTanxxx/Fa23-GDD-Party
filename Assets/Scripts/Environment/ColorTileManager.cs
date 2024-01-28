using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorTileManager : MonoBehaviour
{
    [SerializeField] private TileSpawner[] tileLocs;
    [SerializeField] private int matrixSize;

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

        matrix = new GameObject[matrixSize, matrixSize];

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
                loc = new Vector3(col * 8.94f, 2.5f, row * 8.94f);
            }

            else
            {
                loc = new Vector3(col * 8.94f, 0.11f, row * 8.94f);
            }

            loc += transform.position;

            GameObject placed = Instantiate(obj, loc, Quaternion.identity);

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
        
        //search column
        for (int i = 0; i < matrixSize; i++)
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

        for (int j = 0; j < matrixSize; j++)
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
    
