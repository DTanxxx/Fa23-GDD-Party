using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Lurkers.Environment.Vision
{
    public enum SwitchColor
    {
        Red, //adds red component
        Green, //adds green component
        Blue, //adds blue component
    }

    public class ColorSwitch : MonoBehaviour
    {
        private bool pressed = false;
        [SerializeField] private GameObject[] tileManagers;
        [SerializeField] private SwitchColor sensor;
        [SerializeField] private Light2D glow = null;

        // used to show the area of the trigger(also changes with color of sensor)
        private Collider _collider;
        /*[Header("Gizmo Settings")]
        [SerializeField] private bool _displayGizmos = false;
        [SerializeField] private bool _showOnlyWhileSelected = true;
        [SerializeField] private Color _gizmoColor;*/

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        /*private void OnDrawGizmos()
        {
            if (!_displayGizmos)
            {
                return;
            }

            if (_showOnlyWhileSelected)
            {
                return;
            }

            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
            }
            if (sensor == SensorColor.Red)
            {
                _gizmoColor = new Color(1, 0, 0, 0.5f);
            }
            else if (sensor == SensorColor.Green)
            {
                _gizmoColor = new Color(0, 1, 0, 0.5f);
            }
            else if (sensor == SensorColor.Blue)
            {
                _gizmoColor = new Color(0, 0, 1, 0.5f);
            }
            Gizmos.color = _gizmoColor;
            Gizmos.DrawCube(transform.position, _collider.bounds.size);


        }

        private void OnDrawGizmosSelected()
        {
            if (!_displayGizmos)
            {
                return;
            }

            if (!_showOnlyWhileSelected)
            {
                return;
            }

            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
            }
            if (sensor == SensorColor.Red)
            {
                _gizmoColor = new Color(1, 0, 0, 0.5f);
            }
            else if (sensor == SensorColor.Green)
            {
                _gizmoColor = new Color(0, 1, 0, 0.5f);
            }
            else if (sensor == SensorColor.Blue)
            {
                _gizmoColor = new Color(0, 0, 1, 0.5f);
            }
            Gizmos.color = _gizmoColor;
            Gizmos.DrawCube(transform.position, _collider.bounds.size);

        }*/

        //just in case it may be needed in the future
        public SwitchColor GetSensorColor()
        {
            return sensor;
        }

        //Function to change the colors of tiles when pressed is false(i.e. first time trigger is stepped on)
        //changing of colors is based off what is explained in the design document
        public void TileChangerAdd(SwitchColor s, GameObject t, ColorTileManager ctm, int row, int col)
        {
            TileColor color = t.GetComponent<ColorTile>().GetTileColor();
            bool raised = t.GetComponent<ColorTile>().GetIsRaised();
            switch (s)
            {
                case SwitchColor.Red:
                    if (color == TileColor.White)
                    {
                        t.GetComponent<ColorTile>().TurnRed();
                    }
                    else if (color == TileColor.Blue)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Magenta, ctm, raised);
                        ctm.magentaDictionary.TryAdd(t, (row, col));
                    }
                    else if (color == TileColor.Green)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Yellow, ctm, raised);
                    }
                    break;
                case SwitchColor.Blue:
                    if (color == TileColor.White)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Blue, ctm, raised);
                    }
                    else if (color == TileColor.Red)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Magenta, ctm, raised);
                        ctm.magentaDictionary.TryAdd(t, (row, col));
                    }
                    else if (color == TileColor.Green)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Cyan, ctm, raised);
                    }
                    break;
                case SwitchColor.Green:
                    if (color == TileColor.White)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Green, ctm, raised);
                    }
                    else if (color == TileColor.Red)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Yellow, ctm, raised);
                    }
                    else if (color == TileColor.Blue)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Cyan, ctm, raised);
                    }
                    break;
            }
        }

        //Function that reverts the changes when the trigger was initially pressed(i.e. when pressed is set to true)
        //Colors go back to what they were originally(before the trigger was first activated)
        public void TileChangerRemove(SwitchColor s, GameObject t, ColorTileManager ctm, int row, int col)
        {
            TileColor color = t.GetComponent<ColorTile>().GetTileColor();
            bool raised = t.GetComponent<ColorTile>().GetIsRaised();
            switch (s)
            {
                case SwitchColor.Red:
                    if (color == TileColor.Magenta)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Blue, ctm, raised);
                        ctm.magentaDictionary.Remove(t);
                    }
                    else if (color == TileColor.Yellow)
                    {
                        StartCoroutine(t.GetComponent<ColorTile>().Mover(t, "lower"));
                        t.GetComponent<ColorTile>().SetData(TileColor.Green, ctm, false);
                    }
                    break;
                case SwitchColor.Blue:
                    if (color == TileColor.Magenta)
                    {
                        t.GetComponent<ColorTile>().TurnRed();
                        ctm.magentaDictionary.Remove(t);
                    }
                    else if (color == TileColor.Cyan)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Green, ctm, raised);
                    }
                    break;
                case SwitchColor.Green:
                    if (color == TileColor.Yellow)
                    {
                        StartCoroutine(t.GetComponent<ColorTile>().Mover(t, "lower"));
                        t.GetComponent<ColorTile>().SetData(TileColor.Red, ctm, false);
                    }
                    else if (color == TileColor.Cyan)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Blue, ctm, raised);
                    }
                    break;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            // the case where it hasn't been pressed yet
            if (other.tag == "Player" && pressed == false)
            {
                pressed = true;
                glow.enabled = true;
                foreach (GameObject tiles in tileManagers) // iterating through each present tileManager gameobject in the scene
                {
                    int row = (int)tiles.GetComponent<ColorTileManager>().matrixSize.x;
                    int col = (int)tiles.GetComponent<ColorTileManager>().matrixSize.y;

                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < col; j++) // double for loop to iterate through each tile in each tileManager
                        {
                            if (tiles.GetComponent<ColorTileManager>().matrix[i, j] == null)
                            {
                                continue;
                            }

                            TileChangerAdd(sensor, tiles.GetComponent<ColorTileManager>().matrix[i, j], tiles.GetComponent<ColorTileManager>(), i, j);
                        }
                    }

                }
            }
            // case when it has been pressed
            else if (other.tag == "Player" && pressed == true)
            {
                pressed = false;
                glow.enabled = false;
                foreach (GameObject tiles in tileManagers) // iterating through each present tileManager gameobject in the scene
                {
                    int row = (int)tiles.GetComponent<ColorTileManager>().matrixSize.x;
                    int col = (int)tiles.GetComponent<ColorTileManager>().matrixSize.y;

                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < col; j++) // double for loop to iterate through each tile in each tileManager
                        {
                            if (tiles.GetComponent<ColorTileManager>().matrix[i, j] == null)
                            {
                                continue;
                            }

                            TileChangerRemove(sensor, tiles.GetComponent<ColorTileManager>().matrix[i, j], tiles.GetComponent<ColorTileManager>(), i, j);
                        }
                    }
                }
            }
        }
    }
}
