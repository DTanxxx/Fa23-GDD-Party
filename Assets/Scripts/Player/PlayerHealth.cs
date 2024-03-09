using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lurkers.Control.Vision.Character;

namespace Lurkers.Control
{
    public enum DeathCause
    {
        WEEPINGANGEL,
        FACEHUGGER,
        REDTILE
    }

    public class PlayerHealth : MonoBehaviour
    {
        public static Action<DeathCause, Vector3, GameObject> onDeath;

        private bool isDead = false;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("WeepingAngel"))
            {
                bool enemyActive = collision.gameObject.GetComponentInParent<WeepingAngelController>().IsActive();
                if (onDeath != null && enemyActive)
                {
                    isDead = true;
                    onDeath?.Invoke(DeathCause.WEEPINGANGEL, collision.transform.position, collision.transform.parent.gameObject);
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("FaceHugger"))
            {
                if (onDeath != null)
                {
                    isDead = true;
                    onDeath?.Invoke(DeathCause.FACEHUGGER, collision.transform.position, collision.transform.parent.gameObject);
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
