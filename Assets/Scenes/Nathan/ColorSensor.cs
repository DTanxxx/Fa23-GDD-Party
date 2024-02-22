using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SensorColor
{
    Red, //adds red component
    Green, //adds green component
    Blue, //adds blue component
}

public class ColorSensor : MonoBehaviour
{
    private bool pressed = false;
    private ColorTileManager[] managers;

    private Collider _collider;
    [Header("Gizmo Settings")]
    [SerializeField] private bool _displayGizmos = false;
    [SerializeField] private bool _showOnlyWhileSelected = true;
    [SerializeField] private Color _gizmoColor = new Color(0, 1, 0, 0.5f);

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
        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(transform.position, _collider.bounds.size);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && pressed == false)
        {
            pressed = true;
            
        }
    }
}
