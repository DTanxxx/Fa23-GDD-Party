using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMode : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;
    private GameObject room;
    [SerializeField] private int modeSet;

    // Start is called before the first frame update
    void Start()
    {
        room = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        cameraFollow.setMode(modeSet, room);
    }
}
