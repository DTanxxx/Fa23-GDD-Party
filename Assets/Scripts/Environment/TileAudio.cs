using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Audio.Vision
{
    public class TileAudio : MonoBehaviour
    {
        AudioSource aud;
        Collider2D trigger;
        GameObject player;
        [SerializeField] private float distance;
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
            }
        }
    }
}
