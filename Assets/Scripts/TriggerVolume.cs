using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class TriggerVolume: MonoBehaviour
{
    [Header("Gizmo Settings")]
    [SerializeField] private bool _displayGizmos = false;
    [SerializeField] private bool _showOnlyWhileSelected = true;
    [SerializeField] private Color _gizmoColor = new Color(0, 1, 0, 0.5f);
    

    [Header("Settings")]
    [SerializeField] private bool _oneshot = true;


    public UnityEvent OnEnterTrigger;
    private Collider _collider;
    private bool _alreadyEntered = false;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyEntered && _oneshot)
        {
            return;
        }

        OnEnterTrigger.Invoke();
        _alreadyEntered = true;
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
}
