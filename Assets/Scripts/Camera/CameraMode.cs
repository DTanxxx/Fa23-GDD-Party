using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMode : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private int modeSet;

    private GameObject room;

    private void Start()
    {
        room = this.transform.parent.gameObject;
    }

    private void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        cameraFollow.SetMode(modeSet, room);
    }
}
