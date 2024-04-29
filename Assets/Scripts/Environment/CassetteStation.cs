using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Lurkers.Environment.Hearing
{
    public class CassetteStation : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] private AudioSource audioSource = null;
        [SerializeField] AudioClip audioClip;


        // used to show the area of the trigger(also changes with color of sensor)
        private Collider _collider;
        /*[Header("Gizmo Settings")]
        [SerializeField] private bool _displayGizmos = false;
        [SerializeField] private bool _showOnlyWhileSelected = true;
        [SerializeField] private Color _gizmoColor;*/

        private static bool firstTime = true;

        public static Action onFirstTimeCassette;

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
            _gizmoColor = new Color(0, 0, 1, 0.5f);
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
            _gizmoColor = new Color(0, 0, 1, 0.5f);
            Gizmos.color = _gizmoColor;
            Gizmos.DrawCube(transform.position, _collider.bounds.size);

        }*/


        void OnTriggerEnter(Collider other)
        {
            if (firstTime)
            {
                firstTime = false;
                onFirstTimeCassette?.Invoke();
            }

            if (other.gameObject.CompareTag("Player"))
            {
                button.interactable = true;
                audioSource.clip = audioClip;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                button.interactable = false;
            }
        }
    }
}
