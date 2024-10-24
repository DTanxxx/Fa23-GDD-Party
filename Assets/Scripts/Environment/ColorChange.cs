using Lurkers.Control;
using Lurkers.Control.Vision;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Environment.Vision
{
    public class ColorChange : MonoBehaviour
    {
        private TileColor originalColor;
        private ColorTile colorTile;
        private float timePointedAt = 0f;
        private bool isPointedAt = false;
        private bool isChanged = false;

        private Dictionary<ColorPair, TileColor> combos = new Dictionary<ColorPair, TileColor>();

        [SerializeField] private float timeThres = 2f;
        [SerializeField] private float minDist = 15f;
        [SerializeField] private float angleTolerance = 15f;

        private LightDirection lightDirection;
        private PlayerController playerMovement;
        private ColorTileManager tileManager;

        // Start is called before the first frame update
        void Start()
        {
            colorTile = GetComponent<ColorTile>();
            originalColor = colorTile.GetTileColor();
            Debug.Log(originalColor.ToString());

            lightDirection = GameObject.Find("Flashlight").GetComponent<LightDirection>();
            playerMovement = GameObject.Find("Player").GetComponent<PlayerController>();
            tileManager = this.transform.parent.gameObject.GetComponent<ColorTileManager>();

            combos.Add(new ColorPair("red", TileColor.Black), TileColor.Red);
            combos.Add(new ColorPair("red", TileColor.Blue), TileColor.Magenta);
            combos.Add(new ColorPair("red", TileColor.Green), TileColor.Yellow);
            combos.Add(new ColorPair("red", TileColor.Cyan), TileColor.White);

            combos.Add(new ColorPair("blue", TileColor.Black), TileColor.Blue);
            combos.Add(new ColorPair("blue", TileColor.Red), TileColor.Magenta);
            combos.Add(new ColorPair("blue", TileColor.Green), TileColor.Cyan);
            combos.Add(new ColorPair("blue", TileColor.Yellow), TileColor.White);

            combos.Add(new ColorPair("green", TileColor.Black), TileColor.Green);
            combos.Add(new ColorPair("green", TileColor.Red), TileColor.Yellow);
            combos.Add(new ColorPair("green", TileColor.Blue), TileColor.Cyan);
            combos.Add(new ColorPair("green", TileColor.Magenta), TileColor.White);
        }

        void FixedUpdate()
        {
            float distFromPlayer = Vector3.Distance(playerMovement.transform.position, this.transform.position);
            Debug.Log(distFromPlayer);
            if (distFromPlayer <= minDist)
            {
                Debug.Log("MinRange entered");
                CheckPoint();
            }
            else
            {
                isPointedAt = false;
            }

            if (!isChanged)
            {
                if (isPointedAt)
                {
                    timePointedAt += Time.fixedDeltaTime;
                    if (timePointedAt >= timeThres)
                    {
                        ColorSwitch(lightDirection.GetColor());
                        timePointedAt = timeThres;
                    }
                }
                else
                {
                    timePointedAt = 0f;
                }
            }
            else
            {
                if (!isPointedAt)
                {
                    timePointedAt -= Time.fixedDeltaTime;
                    if (timePointedAt <= 0f)
                    {
                        ColorSwitch("none");
                        timePointedAt = 0f;
                    }
                }
                else
                {
                    timePointedAt = timeThres;
                }
            }

            Debug.Log("Pointed At: " + isPointedAt);
        }

        void CheckPoint()
        {
            Vector3 dirToPlayer = (playerMovement.transform.position - this.transform.position).normalized;
            if (Vector3.Angle(dirToPlayer, playerMovement.GetDir()) >= 180f - angleTolerance)
            {
                isPointedAt = true;
            }
            else
            {
                isPointedAt = false;
            }
        }

        void ColorSwitch(string color)
        {
            TileColor newColor;

            if (!combos.TryGetValue(new ColorPair(color, originalColor), out newColor))
            {
                Debug.Log("No valid combo found for flashColor " + color + " and " + originalColor.ToString());
                if (isChanged)
                {
                    colorTile.TurnColor(originalColor);
                }
                return;
            }

            colorTile.TurnColor(newColor);
        }


    }

    class ColorPair
    {
        string flashColor;
        TileColor tileColor;

        public ColorPair(string flashColor, TileColor tileColor)
        {
            this.flashColor = flashColor;
            this.tileColor = tileColor;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ColorPair other = (ColorPair)obj;
            return flashColor == other.flashColor && tileColor == other.tileColor;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + flashColor.GetHashCode();
                hash = hash * 23 + tileColor.GetHashCode();
                return hash;
            }
        }
    }
}

