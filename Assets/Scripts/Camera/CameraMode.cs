using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Lurkers.Cam
{
    public class CameraMode : MonoBehaviour
    {
        [SerializeField] private CameraFollow[] cameraFollows;
        [SerializeField] private int modeSet;

        private GameObject room;

        private void Start()
        {
            room = this.transform.parent.gameObject;
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (var cam in cameraFollows)
            {
                cam.SetMode(modeSet, room);
            }
        }
    }
}
