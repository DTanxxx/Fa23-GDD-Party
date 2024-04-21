using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDimmer : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private float minIntensity;

    private float o_Intensity;
    private Light flashlight;

    private float currTime;
    
    // Start is called before the first frame update
    void Start()
    {
        flashlight = GetComponent<Light>();
        o_Intensity = flashlight.intensity;
        currTime = maxTime;

        maxTime = maxTime <= 0 ? 180 : maxTime;
        minIntensity = minIntensity < 0 ? 0 : minIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        currTime -= Time.deltaTime;

        currTime = currTime < 0 ? 0 : currTime;

        flashlight.intensity = currTime == 0 ? 0 : (currTime / maxTime) * (o_Intensity - minIntensity);
    }

    public void newBattery()
    {
        currTime = maxTime;
        flashlight.intensity = o_Intensity;
    }
}
