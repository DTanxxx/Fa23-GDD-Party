using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class TileSpawner
{
    public TileColor tileColor;
    public bool raised;
    public GameObject prefab;
    public Vector2Int[] rowCol;

    public TileColor getColor() { return tileColor; }
    public GameObject getPrefab() { return prefab; }
    public Vector2Int[] getRowCol() { return rowCol; }
}