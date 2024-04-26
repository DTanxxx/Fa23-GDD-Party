using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lurkers.Audio.Hearing
{
    public class CassetteAudio : MonoBehaviour
    {
        AudioSource aud;
        [SerializeField] private Button cassetteButton = null;

        private void Start()
        {
            aud = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            cassetteButton.onClick.AddListener(PlayCassette);
        }

        private void OnDisable()
        {
            cassetteButton.onClick.RemoveListener(PlayCassette);
        }

        private void PlayCassette()
        {
            aud.loop = false;
            aud.Stop();
            aud.PlayOneShot(aud.clip);
        }
    }
}
