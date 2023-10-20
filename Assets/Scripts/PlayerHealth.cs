using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static Action<Vector3> onDeath;

    private bool isDead = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WeepingAngel"))
        {
            if (onDeath != null)
            {
                isDead = true;
                onDeath?.Invoke(collision.transform.position);
            }
        }
    }

    public bool GetIsPlayerDead()
    {
        return isDead;
    }
}
