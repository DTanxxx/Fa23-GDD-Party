using Lurkers.Environment.Vision;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.UI
{
    public class ObjectivePanel : MonoBehaviour
    {
        private CanvasGroup invenRender;
        private bool invenVisible = false;

        private void Start()
        {
            invenRender = GetComponent<CanvasGroup>();
            invenRender.interactable = false;
            invenRender.blocksRaycasts = false;
            invenRender.alpha = 0.0f;
        }

        private void OnEnable()
        {
            ElevatorOpen.onElevatorClose += EnablePanel;
        }

        private void OnDisable()
        {
            ElevatorOpen.onElevatorClose -= EnablePanel;
        }

        private void Update()
        {
            //ShowPanel();
        }

        private void EnablePanel()
        {
            /*invenVisible = true;
            invenRender.alpha = 1.0f;
            invenRender.blocksRaycasts = true;*/
        }

        public void ShowPanel()
        {
            if (Input.GetKeyDown("o"))
            {
                if (!invenVisible)
                {
                    EnablePanel();
                }
                else
                {
                    invenVisible = false;
                    invenRender.alpha = 0.0f;
                    invenRender.blocksRaycasts = false;
                }

            }
        }

        public void AddObjective(string text)
        {

        }
    }
}