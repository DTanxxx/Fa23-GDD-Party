using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowObjective : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private CanvasGroup invenRender;
    private bool invenVisible = false;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        invenRender = canvas.GetComponentInChildren<CanvasGroup>();
        invenRender.alpha = 0.0f;
    }

    private void Update()
    {
        ShowPanel();
    }

    public void ShowPanel()
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

    public void AddObjective(string text)
    {

    }
}
