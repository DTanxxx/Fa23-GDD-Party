using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lurkers.Event
{
    public class EnemyAnimationEvents : MonoBehaviour
    {
        public static Action onFacehuggerFootstep;

        // animation event
        public void OnFacehuggerFootstep()
        {
            //onFacehuggerFootstep?.Invoke();
        }
    }
}
