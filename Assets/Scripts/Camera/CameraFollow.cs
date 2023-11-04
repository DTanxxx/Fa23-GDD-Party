using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private int mode; // mode 0 = follow player, mode 1 = lock to room position
    [SerializeField] private Vector3 camOffset = new Vector3(0, 15, -25);

    private Vector3 playerPos;
    private GameObject player;
    private GameObject room;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
        this.transform.rotation = Quaternion.Euler(30, 0, 0);
        this.mode = 0;
    }

    private void Update()
    {
        playerPos = player.transform.position;
        if (mode == 0) 
        {
            this.transform.position = playerPos + camOffset;
            if (room != null)
            {
                GameObject hideGroup = room.transform.Find("HideGroup").gameObject;
                hideGroup.SetActive(true);
            }
        } 
        else if (mode == 1)
        {
            CameraLock cameraLock = room.transform.Find("RoomCameraPosHolder").GetComponent<CameraLock>();
            this.transform.position = cameraLock.GetPosition() + camOffset;
            this.transform.rotation = Quaternion.Euler(cameraLock.GetDegrees(), 0, 0);
            GameObject hideGroup = room.transform.Find("HideGroup").gameObject;
            hideGroup.SetActive(false);
        }
    }

    public int GetMode()
    {
        return mode;
    }

    public void SetMode(int mode, GameObject room)
    {
        this.mode = mode;
        this.room = room;
    }
}
