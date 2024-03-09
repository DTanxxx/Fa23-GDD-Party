using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Camera
{
    public class CameraLock : MonoBehaviour
    {
        [SerializeField] private float degrees;

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public float GetDegrees()
        {
            return degrees;
        }
    }
}
