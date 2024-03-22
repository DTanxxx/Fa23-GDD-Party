using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Hearing;

namespace Lurkers.Audio.Hearing
{
    public class TileAudio : MonoBehaviour
    {
        AudioSource aud;
        Collider2D trigger;
        GameObject player;
        [SerializeField] private float distance;
        [SerializeField] private float soundRange = 100f;
        // Start is called before the first frame update

        void Start()
        {
            aud = GetComponent<AudioSource>();
            trigger = GetComponent<Collider2D>();
            player = GameObject.FindWithTag("Player");
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                aud.Play();

                var sound = new Sound(transform.position, soundRange);
                sound.soundType = Sound.SoundType.Gravel;
                Sounds.MakeSound(sound);
            }
        }
    }
}
