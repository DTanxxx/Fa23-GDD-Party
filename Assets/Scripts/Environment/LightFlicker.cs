using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Light2D spotlight;
    [SerializeField] private Light2D flashlight;
    [SerializeField] private Color desiredColor;
    [SerializeField] private float flickerPerMin = 1f;

    private Color spotColor;
    private Color flashColor;

    private void Start()
    {
        spotColor = spotlight.color;
        flashColor = flashlight.color;

        StartCoroutine(flicker()); 
    }

    private IEnumerator flicker()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(2f);

            ChangeColor(0.8f);
            yield return new WaitForSeconds(1f);

            ChangeColor(0.2f);
            yield return new WaitForSeconds(1f);

            ChangeColor(1f);
            yield return new WaitForSeconds(1f);

            ChangeColor(0f);
            yield return new WaitForSecondsRealtime(60 / flickerPerMin);
        }
    }

    private void ChangeColor(float degree)
    {
        spotlight.color = Color.Lerp(spotColor, desiredColor, degree);
        flashlight.color = Color.Lerp(flashColor, desiredColor, degree);
    }
}
