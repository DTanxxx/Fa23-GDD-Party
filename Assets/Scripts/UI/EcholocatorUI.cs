using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EcholocatorUI : MonoBehaviour
{
    // Variables to store initial state
    private Vector2 initialPosition;
    private Vector2 initialSize;
    private Vector2 imageInitialPosition;
    private Vector2 imageInitialSize;
    private bool expanded = false;

    // Reference variables for child objects
    private RectTransform rectTransform;
    private Image image;
    private TextMeshProUGUI text;
    private RectTransform imageRectTransform;

    [SerializeField] private Vector2 offset;

    void Start()
    {
        // Get references to child objects
        rectTransform = GetComponent<RectTransform>();
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        imageRectTransform = image.GetComponent<RectTransform>();

        // Store initial position and size
        initialPosition = rectTransform.anchoredPosition;
        initialSize = rectTransform.sizeDelta;
        imageInitialPosition = imageRectTransform.anchoredPosition;
        imageInitialSize = imageRectTransform.sizeDelta;

        // Set initial state
        SetInitialState();
    }

    void SetInitialState()
    {
        // Initially hide text field
        text.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        // Check for click input
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse is over the UI element
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out Vector2 localPoint);

            if (rectTransform.rect.Contains(localPoint))
            {
                if (expanded)
                {
                    // If already expanded, revert to initial state
                    SetInitialState();
                    rectTransform.anchoredPosition = initialPosition;
                    rectTransform.sizeDelta = initialSize;
                    imageRectTransform.anchoredPosition = imageInitialPosition;
                    imageRectTransform.sizeDelta = imageInitialSize;
                    expanded = false;
                }
                else
                {
                    // If not expanded, enlarge and show text field
                    text.gameObject.SetActive(true);
                    rectTransform.anchoredPosition = initialPosition + offset;
                    rectTransform.sizeDelta = initialSize * 2;
                    imageRectTransform.anchoredPosition = imageInitialPosition + offset;
                    imageRectTransform.sizeDelta = imageInitialSize * 2;
                    expanded = true;
                }
            }
        }
    }
}
