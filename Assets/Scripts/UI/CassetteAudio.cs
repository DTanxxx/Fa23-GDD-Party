using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lurkers.Audio.Hearing
{
    public class CassetteAudio : MonoBehaviour
    {
        AudioSource aud;
        [SerializeField] private float wait_time;
        [SerializeField] private Button cassetteButton = null;
        [SerializeField] Animator animator;
        
        void Start()
        {
        
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
            aud = GetComponent<AudioSource>();
            aud.loop = false;
            aud.PlayOneShot(aud.clip);
        }
        public void Playing()
        {
            animator.SetBool("ButtonPressed", true);
            Invoke("StopPlay", wait_time);
        }
        void StopPlay()
        {
            animator.SetBool("ButtonPressed", false);
        }
    }
}
