using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameras : MonoBehaviour
{
    // Start is called before the first frame update   
    public List<Camera> camera;
    private bool onCamera;
    private int index;
    void Start() {
        onCamera = false;
        index = 0;
    }
    void Update() 
    {
        if (camera.Count > 0 && Input.GetKeyDown("c")) 
        {
            onCamera = !onCamera;
            camera[index].enabled = !camera[index].enabled;
        }
        if (onCamera && Input.GetKeyDown("e"))
        {
            camera[index].enabled = !camera[index].enabled;
            index += 1;
            if(index >= camera.Count) {
                index = 0;
            }
            camera[index].enabled = !camera[index].enabled;
        }
        if (onCamera && Input.GetKeyDown("q"))
        {
            camera[index].enabled = !camera[index].enabled;
            index -= 1;
            if(index < 0) {
                index = camera.Count - 1;
            }
            camera[index].enabled = !camera[index].enabled;
        }
    }
}
