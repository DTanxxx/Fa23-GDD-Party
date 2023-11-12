using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource = null;


    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public void PlayAudio()
    {
        audioSource.Play();
    }
}
