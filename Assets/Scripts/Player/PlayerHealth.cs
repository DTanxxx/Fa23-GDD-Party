using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lurkers.Character.Player
{
    public enum DeathCause
    {
        WEEPINGANGEL,
        FACEHUGGER,
        REDTILE
    }

    public class PlayerHealth : MonoBehaviour
    {
        public static Action<Vector3, DeathCause> onDeath;

        private bool isDead = false;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("WeepingAngel"))
            {
                bool enemyActive = collision.gameObject.GetComponentInParent<WeepingAngelMovement>().EnemyActivated();
                if (onDeath != null && enemyActive)
                {
                    isDead = true;
                    onDeath?.Invoke(collision.transform.position, DeathCause.WEEPINGANGEL);
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("FaceHugger"))
            {
                if (onDeath != null)
                {
                    isDead = true;
                    onDeath?.Invoke(collision.transform.position, DeathCause.FACEHUGGER);
                }
            }
        }

        public void SetIsPlayerDead()
        {
            isDead = true;
        }

        public bool GetIsPlayerDead()
        {
            return isDead;
        }
    }
}
