using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Lurkers.Environment.Vision
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private Light2D spotlight;
        [SerializeField] private Light2D flashlight;
        [SerializeField] private Color desiredColor;
        [SerializeField] private float darker_red_duration = 5f; // 0.7
        [SerializeField] private float RED_duration = 5f; // 1.0 
        [SerializeField] private float normal_light_duration = 2f; // 0

        private Color spotColor;
        private Color flashColor;

        private void Start()
        {
            spotColor = spotlight.color;
            flashColor = flashlight.color;
        }

        private void OnEnable()
        {
            FlickerTrigger.onFlashlightFlicker += StartFlicker;
        }

        private void OnDisable()
        {
            FlickerTrigger.onFlashlightFlicker -= StartFlicker;
        }

        private void StartFlicker()
        {
            StartCoroutine(flicker());
        }

        private void StopFlicker()
        {
            StopCoroutine(flicker());
        }

        private IEnumerator flicker()
        {
            while (true)
            {

                ChangeColor(1f); // full red
                yield return new WaitForSeconds(RED_duration);
                
                ChangeColor(0f); // normal color 
                yield return new WaitForSecondsRealtime(normal_light_duration);
                
                ChangeColor(0.7f); // darker red
                yield return new WaitForSeconds(darker_red_duration);

                ChangeColor(0f); // normal color 
                yield return new WaitForSecondsRealtime(normal_light_duration);

                
            }
        }

        private void ChangeColor(float degree)
        {
            spotlight.color = Color.Lerp(spotColor, desiredColor, degree);
            flashlight.color = Color.Lerp(flashColor, desiredColor, degree);
        }
    }
}
