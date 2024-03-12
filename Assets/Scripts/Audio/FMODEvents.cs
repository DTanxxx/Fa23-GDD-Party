using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace Lurkers.Audio
{
    public class FMODEvents : MonoBehaviour
    {
        // SFX
        [field: Header("Footsteps SFX")]
        [field: SerializeField] public EventReference footsteps { get; private set; }

        [field: Header("Breathing and Heartbeat SFX")]
        [field: SerializeField] public EventReference breathingAndHeartbeat { get; private set; }

        [field: Header("Elevator SFX")]
        [field: SerializeField] public EventReference elevator { get; private set; }

        [field: Header("Lever SFX")]
        [field: SerializeField] public EventReference lever { get; private set; }

        [field: Header("Tile Activation SFX")]
        [field: SerializeField] public EventReference tileActivation { get; private set; }

        [field: Header("Tile Effect SFX")]
        [field: SerializeField] public EventReference tileEffect { get; private set; }

        [field: Header("Enemy SFX")]
        [field: SerializeField] public EventReference enemy { get; private set; }

        [field: Header("Game Over SFX")]
        [field: SerializeField] public EventReference gameOver { get; private set; }

        // OST
        [field: Header("Main Menu OST")]
        [field: SerializeField] public EventReference mainMenu { get; private set; }

        public static FMODEvents instance { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
    }
}
