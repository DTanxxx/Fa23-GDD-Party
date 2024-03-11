using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Lurkers.UI.Hearing
{
    public class EcholocatorUI : MonoBehaviour
    {
        // Variables to store initial state
        private Vector2 initialPosition;
        private Vector2 initialSize;
        private bool expanded = false;

        // Reference variables for child objects
        private RectTransform rectTransform;
        private TextMeshProUGUI text;

        [SerializeField] private Button echolocatorButton = null;
        [SerializeField] private Animator animator = null;
        [SerializeField] private Vector2 offset;
        [SerializeField] private float upScale = 2f;
        [SerializeField] private float offsetTolerance = 2f;
        [SerializeField] private float zoomRate = 0.1f;

        void Start()
        {
            // Get references to child objects
            rectTransform = GetComponent<RectTransform>();
            text = GetComponentInChildren<TextMeshProUGUI>();

            // Store initial position and size
            initialPosition = rectTransform.anchoredPosition;
            initialSize = rectTransform.localScale;

            text.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            echolocatorButton.onClick.AddListener(OpenEcholocator);
        }

        private void OnDisable()
        {
            echolocatorButton.onClick.RemoveListener(OpenEcholocator);
        }

        private void OpenEcholocator()
        {
            if (expanded)
            {
                // If already expanded, revert to initial state
                StartCoroutine(ZoomOut());
            }
            else
            {
                // If not expanded, enlarge and show text field
                StartCoroutine(ZoomIn());
            }
        }

        private IEnumerator ZoomOut()
        {
            animator.SetBool("Zoom", false);
            echolocatorButton.interactable = false;

            Vector2 curPos = rectTransform.anchoredPosition;
            Vector3 curScale = rectTransform.localScale;
            Vector2 finalPos = initialPosition;
            Vector3 finalScale = initialSize;
            while (Vector2.Distance(curPos, finalPos) > offsetTolerance)
            {
                curPos = Vector2.Lerp(curPos, finalPos, zoomRate);
                curScale = Vector2.Lerp(curScale, finalScale, zoomRate);
                rectTransform.anchoredPosition = curPos;
                rectTransform.localScale = curScale;
                yield return null;
            }

            expanded = false;
            echolocatorButton.interactable = true;
            text.gameObject.SetActive(false);
        }

        private IEnumerator ZoomIn()
        {
            echolocatorButton.interactable = false;

            Vector2 curPos = rectTransform.anchoredPosition;
            Vector3 curScale = rectTransform.localScale;
            Vector2 finalPos = initialPosition + offset;
            Vector3 finalScale = initialSize * upScale;
            while (Vector2.Distance(curPos, finalPos) > offsetTolerance)
            {
                curPos = Vector2.Lerp(curPos, finalPos, zoomRate);
                curScale = Vector2.Lerp(curScale, finalScale, zoomRate);
                rectTransform.anchoredPosition = curPos;
                rectTransform.localScale = curScale;
                yield return null;
            }

            expanded = true;
            text.gameObject.SetActive(true);
            echolocatorButton.interactable = true;

            // also transition animation state
            animator.SetBool("Zoom", true);
        }
    }
}
