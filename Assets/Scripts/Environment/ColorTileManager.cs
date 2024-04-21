using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Environment.Vision.ColorTile
{
    [System.Serializable]
    public struct ChildTileManager
    {
        public ColorTileManager manager;
        public Vector2 originOffset;
    }

    public class ColorTileManager : MonoBehaviour
    {
        [Tooltip("This should match the size of tile prefab's ColorTile transform scales")]
        [SerializeField] private float tileSize = 6f;
        [SerializeField] private TileSpawner[] tileLocs;
        [SerializeField] public Vector2 matrixSize;
        [SerializeField] private ChildTileManager[] childrenTileMan;

        private List<GameObject> redWhiteList = new List<GameObject>();
        private List<GameObject> greenList = new List<GameObject>();
        private List<GameObject> blueList = new List<GameObject>();
        private List<(GameObject, bool)> blackList = new List<(GameObject, bool)>();

        public GameObject[,] matrix;

        private Dictionary<int, GameObject> rowDictionary = new Dictionary<int, GameObject>();
        private Dictionary<int, GameObject> columnDictionary = new Dictionary<int, GameObject>();
        public Dictionary<GameObject, (int, int)> magentaDictionary = new Dictionary<GameObject, (int, int)>();

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

                GameObject placed = Instantiate(obj, transform);
                placed.GetComponent<ColorTile>().SetData(color, this, tile.getIsRaised());
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

                    case TileColor.Blue:
                        blueList.Add(placed);
                        break;
                }

                matrix[row, col] = placed;
            }
        }

        public void ActivateCyan(bool isChild = false)
        {
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

            if (!isChild)
            {
                foreach (var childTileMan in childrenTileMan)
                {
                    childTileMan.manager.ActivateCyan(true);
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

            foreach (var childTileMan in childrenTileMan)
            {
                childTileMan.manager.ActivateMagentaGivenOffset(new Vector2(row, col), childTileMan.originOffset);
            }
        }

        public void ActivateMagentaGivenOffset(Vector2 parentPos, Vector2 childOffset)
        {
            int row = (int)parentPos.x;
            int col = (int)parentPos.y;

            //search row
            for (int i = 0; i < matrixSize.x; i++)
            {
                Vector2 realPos = new Vector2(i, col - childOffset.y);

                if (realPos.y < 0 || realPos.y >= matrix.GetLength(1))
                {
                    // child tile manager's matrix does not contain this tile from parent
                    break;
                }

                GameObject inMatrix = matrix[(int)realPos.x, (int)realPos.y];
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
                Vector2 realPos = new Vector2(row - childOffset.x, j);

                if (realPos.x < 0 || realPos.x >= matrix.GetLength(0))
                {
                    // child tile manager's matrix does not contain this tile from parent
                    break;
                }

                GameObject inMatrix = matrix[(int)realPos.x, (int)realPos.y];
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
        }

        public bool IsAllGreenLowered(bool isChild = false)
        {
            foreach (GameObject tile in greenList)
            {
                ColorTile colorTile = tile.GetComponent<ColorTile>();
                if (colorTile != null)
                {
                    if (colorTile.GetIsRaised())
                    {
                        return false;
                    }
                }
            }

            if (!isChild)
            {
                foreach (var childTileMan in childrenTileMan)
                {
                    if (!childTileMan.manager.IsAllGreenLowered(true))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void ActivateGreen(bool isChild = false)
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

            if (!isChild)
            {
                foreach (var childTileMan in childrenTileMan)
                {
                    childTileMan.manager.ActivateGreen(true);
                }
            }
        }

        public void ResetBlueTileCoroutines(bool isChild = false)
        {
            foreach (GameObject tile in blueList)
            {
                ColorTile colorTile = tile.GetComponent<ColorTile>();
                if (colorTile != null)
                {
                    colorTile.StopSlideCoroutine();
                }
            }

            if (!isChild)
            {
                foreach (var childTileMan in childrenTileMan)
                {
                    childTileMan.manager.ResetBlueTileCoroutines(true);
                }
            }
        }

        public void ActivateBlack(bool isChild = false)
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

            if (!isChild)
            {
                foreach (var childTileMan in childrenTileMan)
                {
                    childTileMan.manager.ActivateBlack(true);
                }
            }
        }
    }
}
    
