using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Environment.Vision.ColorTile
{
    public enum SensorColor
    {
        Red, //adds red component
        Green, //adds green component
        Blue, //adds blue component
    }

    public class ColorSensor : MonoBehaviour
    {
        private bool pressed = false;
        [SerializeField] private GameObject[] tileManagers;
        [SerializeField] private SensorColor sensor;



        // used to show the area of the trigger(also changes with color of sensor)
        private Collider _collider;
        [Header("Gizmo Settings")]
        [SerializeField] private bool _displayGizmos = false;
        [SerializeField] private bool _showOnlyWhileSelected = true;
        [SerializeField] private Color _gizmoColor;


        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnDrawGizmos()
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

        }
        //just in case it may be needed in the future
        public SensorColor GetSensorColor()
        {
            return sensor;
        }

        //Function to change the colors of tiles when pressed is false(i.e. first time trigger is stepped on)
        //changing of colors is based off what is explained in the design document
        public void TileChangerAdd(SensorColor s, GameObject t, ColorTileManager ctm)
        {
            TileColor color = t.GetComponent<ColorTile>().GetTileColor();
            bool raised = t.GetComponent<ColorTile>().GetIsRaised();
            switch (s)
            {
                case SensorColor.Red:
                    if (color == TileColor.White)
                    {
                        t.GetComponent<ColorTile>().TurnRed();
                    }
                    else if (color == TileColor.Blue)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Magenta, ctm, raised);
                    }
                    else if (color == TileColor.Green)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Yellow, ctm, raised);
                    }
                    break;
                case SensorColor.Blue:
                    if (color == TileColor.White)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Blue, ctm, raised);
                    }
                    else if (color == TileColor.Red)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Magenta, ctm, raised);
                    }
                    else if (color == TileColor.Green)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Cyan, ctm, raised);
                    }
                    break;
                case SensorColor.Green:
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
        public void TileChangerRemove(SensorColor s, GameObject t, ColorTileManager ctm)
        {
            TileColor color = t.GetComponent<ColorTile>().GetTileColor();
            bool raised = t.GetComponent<ColorTile>().GetIsRaised();
            switch (s)
            {
                case SensorColor.Red:
                    if (color == TileColor.Magenta)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Blue, ctm, raised);
                    }
                    else if (color == TileColor.Yellow)
                    {
                        StartCoroutine(t.GetComponent<ColorTile>().Mover(t, "lower"));
                        t.GetComponent<ColorTile>().SetData(TileColor.Green, ctm, false);
                    }
                    break;
                case SensorColor.Blue:
                    if (color == TileColor.Magenta)
                    {
                        t.GetComponent<ColorTile>().TurnRed();
                    }
                    else if (color == TileColor.Cyan)
                    {
                        t.GetComponent<ColorTile>().SetData(TileColor.Green, ctm, raised);
                    }
                    break;
                case SensorColor.Green:
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
                foreach (GameObject tiles in tileManagers) // iterating through each present tileManager gameobject in the scene
                {
                    int row = (int)tiles.GetComponent<ColorTileManager>().matrixSize.x;
                    int col = (int)tiles.GetComponent<ColorTileManager>().matrixSize.y;

                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < col; j++) // double for loop to iterate through each tile in each tileManager
                        {
                            TileChangerAdd(sensor, tiles.GetComponent<ColorTileManager>().matrix[i, j], tiles.GetComponent<ColorTileManager>());
                        }
                    }

                }
            }
            // case when it has been pressed
            else if (other.tag == "Player" && pressed == true)
            {
                pressed = false;
                foreach (GameObject tiles in tileManagers) // iterating through each present tileManager gameobject in the scene
                {
                    int row = (int)tiles.GetComponent<ColorTileManager>().matrixSize.x;
                    int col = (int)tiles.GetComponent<ColorTileManager>().matrixSize.y;

                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < col; j++) // double for loop to iterate through each tile in each tileManager
                        {
                            TileChangerRemove(sensor, tiles.GetComponent<ColorTileManager>().matrix[i, j], tiles.GetComponent<ColorTileManager>());
                        }
                    }

                }
            }
        }
    }
}
