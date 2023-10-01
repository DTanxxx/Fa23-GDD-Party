using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowInven : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    private CanvasGroup invenRender;
    private bool invenVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        invenRender = canvas.GetComponentInChildren<CanvasGroup>();
        invenRender.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            if (!invenVisible)
            {
                invenVisible = true;
                invenRender.alpha = 1.0f;
            }

            else
            {
                invenVisible = false;
                invenRender.alpha = 0.0f;
            }

        }


    }
}
