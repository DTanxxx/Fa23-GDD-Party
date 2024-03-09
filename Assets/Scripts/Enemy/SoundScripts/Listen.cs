using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Hearing
{
    // To implement in all objects that want to listen for sound
    public interface Listen
    {
        void RespondToSound(Sound sound);
    }
}
