using Lurkers.Control.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lurkers.UI
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private Button continueButton = null;
        [SerializeField] private Button restartButton = null;

        private CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            LevelManager.onPauseGame += ShowPanel;
            LevelManager.onUnpausegame += HidePanel;

            continueButton.onClick.AddListener(LevelManager.Instance.UnpauseGame);
            restartButton.onClick.AddListener(LevelManager.Instance.UnpauseGame);
            restartButton.onClick.AddListener(LevelManager.Instance.StopAllCoroutines);
            restartButton.onClick.AddListener(LevelManager.Instance.ReloadScene);
        }

        private void OnDisable()
        {
            LevelManager.onPauseGame -= ShowPanel;
            LevelManager.onUnpausegame -= HidePanel;

            continueButton.onClick.RemoveListener(LevelManager.Instance.UnpauseGame);
            restartButton.onClick.RemoveListener(LevelManager.Instance.UnpauseGame);
            restartButton.onClick.RemoveListener(LevelManager.Instance.StopAllCoroutines);
            restartButton.onClick.RemoveListener(LevelManager.Instance.ReloadScene);
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
