using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Lurkers.Control;

namespace Lurkers.UI.Vision
{
    public class SecurityCameras : MonoBehaviour
    {
        [SerializeField] private Button cctvButton = null; 
        [SerializeField] private List<GameObject> cam;

        private bool onCamera;
        private int index;
        private PlayerController player;

        void Start()
        {
            onCamera = false;
            index = 0;
            player = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            SecurityCamEnabler.onSecurityCamPickup += EnableCam;

            cctvButton.onClick.AddListener(OpenCCTV);
        }

        private void OnDisable()
        {
            SecurityCamEnabler.onSecurityCamPickup -= EnableCam;

            cctvButton.onClick.RemoveListener(OpenCCTV);
        }

        private void EnableCam()
        {
            cctvButton.gameObject.SetActive(true);
        }

        private void OpenCCTV()
        {
            if (cam.Count == 0)
            {
                return;
            }

            if (onCamera)
            {
                player.UnfreezePlayer();
            }
            else
            {
                player.FreezePlayer();
            }

            onCamera = !onCamera;
            cam[index].SetActive(!cam[index].activeSelf);
        }

        void Update()
        {
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
