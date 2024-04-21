using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Cam;

namespace Lurkers.Environment.Vision
{
    public class FocusLight : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            CameraFollow.onCameraShiftComplete += BeginFlickeringSequence;
        }

        private void OnDisable()
        {
            CameraFollow.onCameraShiftComplete -= BeginFlickeringSequence;
        }

        private void BeginFlickeringSequence()
        {
            // animate the light source and overhead lamp sprite
            animator.SetTrigger("Flicker");
        }
    }
}
