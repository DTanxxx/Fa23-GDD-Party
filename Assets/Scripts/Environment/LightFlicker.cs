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
        [SerializeField] private float flickerPerMin = 12f;

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
                yield return new WaitForSeconds(1f);

                ChangeColor(0.7f);
                yield return new WaitForSeconds(0.75f);

                ChangeColor(0.3f);
                yield return new WaitForSeconds(0.75f);

                ChangeColor(1f);
                yield return new WaitForSeconds(0.75f);

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
}
