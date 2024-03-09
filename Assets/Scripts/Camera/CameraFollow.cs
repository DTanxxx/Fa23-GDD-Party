using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lurkers.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private int mode; // mode 0 = follow player, mode 1 = lock to room position, mode 2 = cinematic sequence
        [SerializeField] private Vector3 camOffset = new Vector3(0, 15, -25);
        [SerializeField] private Transform focusTransform = null;  // transform to look at during lever pull sequence
        [SerializeField] private float cameraPauseDuration = 2f;
        [Range(0f, 1f)] [SerializeField] private float cameraShiftRate = 0.1f;
        [SerializeField] private float focusReachDistance = 1f;

        private Vector3 playerPos;
        private GameObject player;
        private GameObject room;
        private WaitForSeconds waitForCameraPause;
        private CameraLock cameraLock;

        public static Action onCameraShiftComplete;
        public static Action onCameraRestoreComplete;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerPos = player.transform.position;
            this.transform.rotation = Quaternion.Euler(30, 0, 0);
            this.mode = 0;
            waitForCameraPause = new WaitForSeconds(cameraPauseDuration);
        }

        private void OnEnable()
        {
            LeverPullAnimationEvents.onBeginLeverCinematicSequence += StartCameraSequence;
            FocusLightAnimationEvents.onFocusLightFinishAnim += RestoreCameraSequence;
        }

        private void OnDisable()
        {
            LeverPullAnimationEvents.onBeginLeverCinematicSequence -= StartCameraSequence;
            FocusLightAnimationEvents.onFocusLightFinishAnim -= RestoreCameraSequence;
        }

        private void StartCameraSequence()
        {
            StartCoroutine(CameraShift());
        }

        private IEnumerator CameraShift()
        {
            // camera pauses for 2 seconds
            yield return waitForCameraPause;

            // adjust camera modes to allow it shifting out of room view
            mode = 2;
        }

        private void RestoreCameraSequence()
        {
            StartCoroutine(RestoreCamera());
        }

        private IEnumerator RestoreCamera()
        {
            yield return waitForCameraPause;

            // camera restore
            mode = 3;
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
                /*cameraLock = room.transform.Find("RoomCameraPosHolder").GetComponent<CameraLock>();
                this.transform.position = cameraLock.GetPosition();
                this.transform.rotation = Quaternion.Euler(cameraLock.GetDegrees(), 0, 0);
                GameObject hideGroup = room.transform.Find("HideGroup").gameObject;
                hideGroup.SetActive(false);*/
            }
            else if (mode == 2)
            {
                float camDist = Vector3.Distance(transform.position, focusTransform.position + camOffset);
                if (camDist <= focusReachDistance)
                {
                    return;
                }

                // shift camera to look at focusTransform
                transform.position = Vector3.Lerp(transform.position,
                    focusTransform.position + camOffset, cameraShiftRate);

                camDist = Vector3.Distance(transform.position, focusTransform.position + camOffset);
                if (camDist <= focusReachDistance)
                {
                    // camera shift sequence complete
                    onCameraShiftComplete?.Invoke();
                }
            }
            else if (mode == 3)
            {
                Vector3 targetPos = playerPos + camOffset;
                float camDist = Vector3.Distance(transform.position, targetPos);
                if (camDist <= focusReachDistance)
                {
                    return;
                }

                //cameraLock = room.transform.Find("RoomCameraPosHolder").GetComponent<CameraLock>();

                // shift camera to go back to player pos
                transform.position = Vector3.Lerp(transform.position,
                    targetPos, cameraShiftRate);

                camDist = Vector3.Distance(transform.position, targetPos);
                if (camDist <= focusReachDistance)
                {
                    // camera shift sequence complete
                    onCameraRestoreComplete?.Invoke();
                    mode = 0;
                }
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
}
