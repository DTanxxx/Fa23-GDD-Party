using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Hearing;
using Lurkers.Control;

namespace Lurkers.Audio.Hearing
{
    public class TileAudio : MonoBehaviour
    {
        AudioSource aud;
        Collider2D trigger;
        Collider another;
        bool isOn;
        bool moving;
        [SerializeField] private float distance;
        [SerializeField] private float soundRange = 100f;
        // Start is called before the first frame update

        void Start()
        {
            aud = GetComponent<AudioSource>();
            trigger = GetComponent<Collider2D>();
        }
        private void OnEnable()
        {
            PlayerController.moving += isMoving;
            PlayerController.notMoving += notMove;
        }
        private void OnDisable()
        {
            PlayerController.moving -= isMoving;
            PlayerController.notMoving -= notMove;
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                aud.Play();
                isOn = true;
                var sound = new Sound(transform.position, soundRange);
                sound.soundType = Sound.SoundType.Gravel;
                Sounds.MakeSound(sound);
                another = other;
            }
        }
        void OnTriggerExit(Collider other) {
            isOn = false;
        }
        void Update()
        {

            if (moving && isOn && !aud.isPlaying)
            {
                    aud.Play();
            }
        }
        void isMoving() {
            moving = true;
        }
        void notMove() {
            moving = false;
        }
    }
}
