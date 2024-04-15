using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Cassette : MonoBehaviour
{
    
    [SerializeField] Button button;
    [SerializeField] private AudioSource audioSource = null;
    //[SerializeField] GameObject play_Button;
    [SerializeField] AudioClip audio;
    
    
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

        }
    
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Button real_button = button.GetComponent<Button>();
            button.interactable = true;
            audioSource.clip = audio;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            /*Button real_button = button.GetComponent<Button>();
            real_button.interactable = false;*/
            button.interactable = false;
        }
    }
}
