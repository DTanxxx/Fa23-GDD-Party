
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Objective
{
    public abstract class Goal : MonoBehaviour
    {
        public abstract bool IsAchieved();
        public abstract void Complete();
        public abstract void AddtoUI();
    }
}
