using Lurkers.Control.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.UI
{
    public class PausePanel : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            LevelManager.onPauseGame += ShowPanel;
            LevelManager.onUnpausegame += HidePanel;
        }

        private void OnDisable()
        {
            LevelManager.onPauseGame -= ShowPanel;
            LevelManager.onUnpausegame -= HidePanel;
        }

        private void ShowPanel()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void HidePanel()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
