using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Environment.Vision
{
    public class Elevator : MonoBehaviour
    {
        public enum State
        {
            OPEN = 0,
            OPEN_CLOSE = 1,
            MOVING = 2,
        }
    }
}
