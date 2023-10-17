using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] Goal[] goals;

    private void Awake()
    {
        goals = GetComponents<Goal>();
    }

    // Update is called once per frame
    void Update()
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
