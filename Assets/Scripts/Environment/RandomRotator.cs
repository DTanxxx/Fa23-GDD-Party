using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Environment
{
    public class RandomRotator : MonoBehaviour
    {
        private void Start()
        {
            // apply a random rotation out of 360 degrees
            transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        }
    }
}
