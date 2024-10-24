using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Environment.Vision
{
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
        public bool getIsRaised() { return raised; }
    }
}