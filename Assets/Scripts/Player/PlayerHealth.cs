using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lurkers.Control.Vision;
using Lurkers.Control.Level;

namespace Lurkers.Control
{
    public enum DeathCause
    {
        WEEPINGANGEL,
        FACEHUGGER,
        REDTILE,
        CTHULHU,
        MOTHMAN
    }

    public class PlayerHealth : MonoBehaviour
    {
        public static Action<DeathCause, Vector3, GameObject> onDeath;

        private bool isDead = false;
        private bool isImmune = false;

        private void OnEnable()
        {
            NextLevelTrigger.onBeginLevelTransition += PlayerImmune;
        }

        private void OnDisable()
        {
            NextLevelTrigger.onBeginLevelTransition -= PlayerImmune;
        }

        private void PlayerImmune()
        {
            isImmune = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isImmune)
            {
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("WeepingAngel"))
            {
                bool enemyActive = other.gameObject.GetComponentInParent<WeepingAngelController>().IsActive();
                if (onDeath != null && enemyActive)
                {
                    isDead = true;
                    onDeath?.Invoke(DeathCause.WEEPINGANGEL, other.transform.position, other.transform.parent.gameObject);
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("FaceHugger"))
            {
                if (onDeath != null)
                {
                    isDead = true;
                    onDeath?.Invoke(DeathCause.FACEHUGGER, other.transform.position, other.transform.parent.gameObject);
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Cthulhu"))
            {
                if (onDeath != null)
                {
                    isDead = true;
                    onDeath?.Invoke(DeathCause.CTHULHU, other.transform.position, other.transform.parent.gameObject);
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("MothMan"))
            {
                if (onDeath != null)
                {
                    isDead = true;
                    onDeath?.Invoke(DeathCause.MOTHMAN, other.transform.position, other.transform.parent.gameObject);
                }
            }
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            if (isImmune)
            {
                return;
            }

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
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Cthulhu"))
            {
                if (onDeath != null)
                {
                    isDead = true;
                    onDeath?.Invoke(DeathCause.CTHULHU, collision.transform.position, collision.transform.parent.gameObject);
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("MothMan"))
            {
                if (onDeath != null)
                {
                    isDead = true;
                    onDeath?.Invoke(DeathCause.MOTHMAN, collision.transform.position, collision.transform.parent.gameObject);
                }
            }
        }*/

        public void SetIsPlayerDead()
        {
            isDead = true;
        }

        public bool GetIsPlayerDead()
        {
            return isDead;
        }

        public void SetIsPlayerAlive()
        {
            isDead = false;
        }
    }
}
