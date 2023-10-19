using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour

{
    private Vector3 playerPos;
    private GameObject player;
    [SerializeField] private int mode; // mode 0 = follow player, mode 1 = lock to room position
    private GameObject room;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
        this.transform.rotation = Quaternion.Euler(30, 0, 0);
        this.mode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        if (mode == 0) 
        {
            this.transform.position = playerPos + new Vector3(0, 15, -25);
            if (room != null)
            {
                GameObject hideGroup = room.transform.Find("HideGroup").gameObject;
                hideGroup.SetActive(true);
            }
        } else if (mode == 1)
        {
            CameraLock cameraLock = room.transform.Find("RoomCameraPosHolder").GetComponent<CameraLock>();
            this.transform.position = cameraLock.getPosition();
            this.transform.rotation = Quaternion.Euler(cameraLock.getDegrees(), 0, 0);
            GameObject hideGroup = room.transform.Find("HideGroup").gameObject;
            hideGroup.SetActive(false);
        }
    }

    public int getMode()
    {
        return mode;
    }

    public void setMode(int mode, GameObject room)
    {
        this.mode = mode;
        this.room = room;
    }
}
