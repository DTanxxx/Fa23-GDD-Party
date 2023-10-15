using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static Action onDeath;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WeepingAngel"))
        {
            if (onDeath != null)
            {
                onDeath?.Invoke();
            }
        }
    }
}
