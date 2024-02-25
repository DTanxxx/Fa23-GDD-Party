using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteAudio : MonoBehaviour
{   
    AudioSource aud;
    [SerializeField] private float wait_time;
    // Start is called before the first frame update
    void OnEnable()
    {
        aud = GetComponent<AudioSource>();
        aud.loop = true;
        aud.Play();
        Invoke("StopPlay", wait_time);
    }
    void StopPlay()
    {
        aud.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
