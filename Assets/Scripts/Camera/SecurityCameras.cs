using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Environment.Vision
{
    public class SecurityCameras : MonoBehaviour
    {
        // Start is called before the first frame update   
        public List<Camera> cam;
        private bool onCamera;
        private int index;
        void Start()
        {
            onCamera = false;
            index = 0;
        }
        void Update()
        {
            if (cam.Count > 0 && Input.GetKeyDown("c"))
            {
                onCamera = !onCamera;
                cam[index].enabled = !cam[index].enabled;
            }
            if (onCamera && Input.GetKeyDown("e"))
            {
                cam[index].enabled = !cam[index].enabled;
                index += 1;
                if (index >= cam.Count)
                {
                    index = 0;
                }
                cam[index].enabled = !cam[index].enabled;
            }
            if (onCamera && Input.GetKeyDown("q"))
            {
                cam[index].enabled = !cam[index].enabled;
                index -= 1;
                if (index < 0)
                {
                    index = cam.Count - 1;
                }
                cam[index].enabled = !cam[index].enabled;
            }
        }
    }
}
