using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Objective
{
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField] private Goal[] goals;

        private void Awake()
        {
            goals = GetComponents<Goal>();
        }

        private void Update()
        {
            foreach (Goal goal in goals)
            {
                if (goal.IsAchieved())
                {
                    goal.Complete();
                    Destroy(goal);
                }
            }
        }
    }
}
