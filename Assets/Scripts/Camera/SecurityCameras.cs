using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.UI.Vision;
using System;

namespace Lurkers.Environment.Vision
{
    public class SecurityCameras : MonoBehaviour
    {
        // Start is called before the first frame update   
        public List<GameObject> cam;
        private bool onCamera;
        private int index;

        public bool camEnabled = false;

        public static Action freezeOn;
        public static Action freezeOff;
        void Start()
        {
            onCamera = false;
            index = 0;
        }

        private void OnEnable()
        {
            SecurityCamEnabler.onSecurityCamPickup += EnableCam;    
        }

        private void OnDisable()
        {
            SecurityCamEnabler.onSecurityCamPickup -= EnableCam;
        }

        private void EnableCam()
        {
            camEnabled = true;
        }

        void Update()
        {
            if (!camEnabled)
            {
                return;
            }

            if (cam.Count > 0 && Input.GetKeyDown("c"))
            {
                //Debug.Log(onCamera);
                switch (!onCamera)
                {
                    case true:
                        freezeOn?.Invoke();
                        break;
                    case false:
                        freezeOff?.Invoke();
                        break;
                }
                onCamera = !onCamera;
                cam[index].SetActive(!cam[index].activeSelf);

            }
            if (onCamera && Input.GetKeyDown("e"))
            {
                cam[index].SetActive(!cam[index].activeSelf);
                index += 1;
                if (index >= cam.Count)
                {
                    index = 0;
                }
                cam[index].SetActive(!cam[index].activeSelf);
            }
            if (onCamera && Input.GetKeyDown("q"))
            {
                cam[index].SetActive(!cam[index].activeSelf);
                index -= 1;
                if (index < 0)
                {
                    index = cam.Count - 1;
                }
                cam[index].SetActive(!cam[index].activeSelf);
            }
        }
    }
}
